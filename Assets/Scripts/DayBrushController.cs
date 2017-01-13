using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Drives the functionality for DayBrush.
/// </summary>
public class DayBrushController : MonoBehaviour {

    public GameObject pencil;
    public GameObject paint;
    public float strokeOffsetZ = -0.5f;
    public AudioClip undoSFX;
    public AudioClip redoSFX;

    private Stack<GameObject> strokes;
    private Stack<GameObject> undoneStrokes;
    private GvrControllerGesture gesture;
    private Material paintMaterial;

    void Awake ()
    {
        strokes = new Stack<GameObject>();
        undoneStrokes = new Stack<GameObject>();
        paintMaterial = paint.gameObject.GetComponent<TrailRenderer>().material;
        NextPaint();
    }

    void Update ()
    {
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
        GameObject stroke = GameObject.Instantiate<GameObject>(paint);
        stroke.transform.SetParent(pencil.transform);
        stroke.transform.localPosition = new Vector3(0, 0, strokeOffsetZ);
        stroke.gameObject.GetComponent<TrailRenderer>().enabled = true;
        strokes.Push(stroke);
    }

    private void EndStroke ()
    {
        GameObject stroke = strokes.Peek();
        stroke.transform.SetParent(this.transform);
        undoneStrokes = new Stack<GameObject>();
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
        pencil.gameObject.GetComponent<MeshRenderer>().material.color = newColor;
        Material newPaintMaterial = Material.Instantiate(paintMaterial);
        newPaintMaterial.SetColor("_EmissionColor", newColor);
        paint.gameObject.GetComponent<TrailRenderer>().material = newPaintMaterial;
    }

    private void UndoStroke ()
    {
        if (strokes.Count > 0) {
            GameObject stroke = strokes.Pop();
            stroke.SetActive(false);
            undoneStrokes.Push(stroke);
            AudioSource.PlayClipAtPoint(undoSFX, stroke.transform.position);
        }
    }

    private void RedoStroke ()
    {
        if (undoneStrokes.Count > 0) {
            GameObject stroke = undoneStrokes.Pop();
            stroke.SetActive(true);
            strokes.Push(stroke);
            AudioSource.PlayClipAtPoint(redoSFX, stroke.transform.position);
        }
    }

    private void SavePainting ()
    {
        Painting p = new Painting();
        p.name = "testPainting";
        p.color = strokes.Peek().GetComponent<TrailRenderer>().material.GetColor("_EmissionColor");
        Storage.Save(p);
    }

    private void LoadPainting ()
    {
        Painting p = Storage.Load();
        if (p != null) {
            pencil.gameObject.GetComponent<MeshRenderer>().material.color = p.color;
        }
    }
}
