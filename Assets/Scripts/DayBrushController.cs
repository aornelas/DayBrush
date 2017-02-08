using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Drives the functionality for DayBrush.
/// </summary>
// TODO: fix redo of large strokes, only first segment loads
// TODO: fix painting loading stroke order, flips on every save 
public class DayBrushController : MonoBehaviour {

    public GameObject pencil;
    public GameObject paint;
    public GameObject floorButtons;
    public AudioClip undoSFX;
    public AudioClip redoSFX;

    private Stack<Stroke> strokesDone;
    private Stack<Stroke> strokesUndone;
    private GvrControllerGesture gesture;
    private Material paintMaterial;
    private GameObject loadingPencil;
    private Stroke loadingStroke;
    private Color currentColor;
    private Teleporter teleporter;

    /// Status flags
    private bool _isPainting;
    private bool _isLoadingStroke;
    private bool _isLoadingPainting;

    void Awake ()
    {
        strokesDone = new Stack<Stroke>();
        strokesUndone = new Stack<Stroke>();
        paintMaterial = paint.gameObject.GetComponent<TrailRenderer>().material;
        NextPaint();

        teleporter = pencil.gameObject.GetComponent<Teleporter>();
    }

    void Update ()
    {
        if (floorButtons.activeSelf) {
            return;
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
            if (gesture != null) {
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
        }

        if (GvrController.AppButtonDown) {
//            if (GvrController.IsTouching) {
//                LoadPainting();
//            } else {
//                SavePainting();
//            }
        }
    }

    void FixedUpdate ()
    {
        if (_isLoadingStroke || _isLoadingPainting) {
            if (loadingStroke.HasMorePoints()) {
                loadingPencil.transform.position = loadingStroke.NextPoint();
            } else {
                loadingStroke.FinishLoading();
                _isLoadingStroke = false;
                if (_isLoadingPainting) {
                    if (strokesUndone.Count > 0) {
                        RedoStroke();
                    } else {
                        _isLoadingPainting = false;
                    }
                }
            }
        }
    }

    void LateUpdate ()
    {
        if (_isPainting) {
            strokesDone.Peek().RecordPoint();
        }
    }

    public void ClearPainting () {
        while (strokesDone.Count > 0) {
            UndoStroke();
        }
        strokesUndone = new Stack<Stroke>();
    }

    private void StartStroke ()
    {
        if (!_isLoadingStroke && !_isLoadingPainting) {
            _isPainting = true;
            
            Stroke stroke = new Stroke(this.transform.parent, pencil.transform, paint);
            stroke.StartPainting();
            strokesDone.Push(stroke);

            teleporter.RecordPosition();
        }
    }

    private void EndStroke ()
    {
        if (!_isLoadingStroke && !_isLoadingPainting) {
            _isPainting = false;

            if (strokesDone.Count > 0) {
                Stroke stroke = strokesDone.Peek();
                stroke.StopPainting();
                strokesUndone = new Stack<Stroke>();
            }
        }
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
        if (strokesDone.Count > 0) {
            ExpediteStrokeLoading();
            Stroke stroke = strokesDone.Pop();
            stroke.Hide();
            strokesUndone.Push(stroke);
            if (!_isLoadingPainting) {
                AudioSource.PlayClipAtPoint(undoSFX, this.transform.position);
            }
        }
    }

    private void RedoStroke ()
    {
        ExpediteStrokeLoading();
        if (strokesUndone.Count > 0) {
            _isLoadingStroke = true;
            Stroke stroke = strokesUndone.Pop();
            loadingPencil = GameObject.Instantiate<GameObject>(pencil);
            loadingPencil.GetComponent<Teleporter>().enabled = false;
            loadingStroke = stroke;
            loadingStroke.StartLoading(loadingPencil);
            strokesDone.Push(stroke);
            if (!_isLoadingPainting) {
                AudioSource.PlayClipAtPoint(redoSFX, this.transform.position);
            }
        }
    }
    
    private void ExpediteStrokeLoading ()
    {
        if (_isLoadingStroke) {
            loadingStroke.FinishLoading();
            _isLoadingStroke = false;
        }
    }

    private void SavePainting ()
    {
        PaintingData p = new PaintingData();
        p.name = "testPainting";
        List<List<StrokeData>> paintingStrokeData = new List<List<StrokeData>>();
        foreach (Stroke s in strokesDone) {
            paintingStrokeData.Add(s.GetStrokeData());
        }
        p.strokes = paintingStrokeData;
        Storage.Save(p);
    }

    private void LoadPainting ()
    {
        _isLoadingPainting = true;
        PaintingData p = Storage.Load();
        if (p != null) {
            ClearPainting();

            Queue<Stroke> paintingStrokes = new Queue<Stroke>();
            foreach (List<StrokeData> strokeDataList in p.strokes) {
                SetPaint(strokeDataList[0].color);
                Stroke stroke = new Stroke(this.transform, pencil.transform, paint);
                stroke.SetStrokeData(strokeDataList);
                paintingStrokes.Enqueue(stroke);
            }

            while (paintingStrokes.Count > 0) {
                strokesUndone.Push(paintingStrokes.Dequeue());
            }

            RedoStroke();
        }
    }
}
