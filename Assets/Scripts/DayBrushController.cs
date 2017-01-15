using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Drives the functionality for DayBrush.
/// </summary>
public class DayBrushController : MonoBehaviour {

    public GameObject pencil;
    public GameObject paint;
    public AudioClip undoSFX;
    public AudioClip redoSFX;

    private Stack<Stroke> strokes;
    private Stack<Stroke> undoneStrokes;
    private GvrControllerGesture gesture;
    private Material paintMaterial;
    private GameObject loadingPencil;
    private Stroke loadingStroke;
    private Color currentColor;

    /// Status flags
    private bool _isPainting;
    private bool _isLoading;

    void Awake ()
    {
        strokes = new Stack<Stroke>();
        undoneStrokes = new Stack<Stroke>();
        paintMaterial = paint.gameObject.GetComponent<TrailRenderer>().material;
        NextPaint();
    }

    void Update ()
    {
        if (_isPainting) {
            strokes.Peek().RecordPoint();
        }

        if (_isLoading) {
            if (loadingStroke.HasMorePoints()) {
                loadingPencil.transform.position = loadingStroke.NextPoint();
            } else {
                loadingStroke.FinishLoading();
                _isLoading = false;
            }
        }

        if (GvrController.ClickButtonDown) {
            StartStroke();
        }

        if (GvrController.ClickButtonUp) {
            EndStroke();
        }

        if (GvrController.TouchDown) {
            gesture = new GvrControllerGesture();
        }

        if (GvrController.TouchUp) {
            if (gesture.SwipedLeft()) {
                UndoStroke();
            } else if (gesture.SwipedRight()) {
                RedoStroke();
            } else if (gesture.SwipedDown()) {
                NextPaint();
            } else if (gesture.SwipedUp()) {
                PreviousPaint();
            }
        }

        if (GvrController.AppButtonDown) {
            if (GvrController.IsTouching) {
                LoadPainting();
            } else {
                SavePainting();
            }
        }
    }

    private void StartStroke ()
    {
        _isPainting = true;

        Stroke stroke = new Stroke(this.transform, pencil.transform, paint);
        stroke.StartPainting();
        strokes.Push(stroke);
    }

    private void EndStroke ()
    {
        _isPainting = false;

        Stroke stroke = strokes.Peek();
        stroke.StopPainting();
        undoneStrokes = new Stack<Stroke>();
    }

    private void NextPaint ()
    {
        SetPaint(Paint.NextColor());
    }

    private void PreviousPaint ()
    {
        SetPaint(Paint.PreviousColor());
    }

    private void SetPaint (Color newColor)
    {
        currentColor = newColor;
        pencil.gameObject.GetComponent<MeshRenderer>().material.color = newColor;
        Material newPaintMaterial = Material.Instantiate(paintMaterial);
        newPaintMaterial.SetColor("_EmissionColor", newColor);
        paint.gameObject.GetComponent<TrailRenderer>().material = newPaintMaterial;
    }

    private void UndoStroke ()
    {
        if (strokes.Count > 0) {
            ExpediteStrokeLoading();
            Stroke stroke = strokes.Pop();
            stroke.Hide();
            undoneStrokes.Push(stroke);
            AudioSource.PlayClipAtPoint(undoSFX, this.transform.position);
        }
    }

    private void RedoStroke ()
    {
        ExpediteStrokeLoading();
        if (undoneStrokes.Count > 0) {
            Stroke stroke = undoneStrokes.Pop();
            loadingPencil = GameObject.Instantiate<GameObject>(pencil);
            loadingStroke = stroke;
            loadingStroke.StartLoading(loadingPencil);
            _isLoading = true;
            strokes.Push(stroke);
            AudioSource.PlayClipAtPoint(redoSFX, this.transform.position);
        }
    }
    
    private void ExpediteStrokeLoading ()
    {
        if (_isLoading) {
            loadingStroke.FinishLoading();
            _isLoading = false;
        }
    }

    private void SavePainting ()
    {
        PaintingData p = new PaintingData();
        p.name = "testPainting";
        p.color = currentColor;
        Storage.Save(p);
    }

    private void LoadPainting ()
    {
        PaintingData p = Storage.Load();
        if (p != null) {
            pencil.gameObject.GetComponent<MeshRenderer>().material.color = p.color;
        }
    }
}
