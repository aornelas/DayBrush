using UnityEngine;
using System.Collections;

public class Stroke {

    private static int maxPoints = 100;
    private static float strokeOffsetZ = -0.5f;

    private Transform canvas;
    private Transform pencil;
    private GameObject paint;
    private Vector3[] points;
    private int nextPointIndex;

    public Stroke (Transform canvas, Transform pencil, GameObject paint)
    {
        this.canvas = canvas;
        this.pencil = pencil;

        this.paint = GameObject.Instantiate<GameObject>(paint);

        points = new Vector3[maxPoints];
        nextPointIndex = 0;
    }

    public void StartPainting ()
    {
        paint.transform.SetParent(pencil);
        paint.transform.localPosition = new Vector3(0, 0, strokeOffsetZ);
        paint.gameObject.GetComponent<TrailRenderer>().enabled = true;
    }

    public void StopPainting ()
    {
        paint.transform.SetParent(canvas);
    }

    public void Hide ()
    {
        paint.SetActive(false);
    }

    public void Show ()
    {
        paint.SetActive(true);
    }

    public void AddPoint (Vector3 point)
    {
        points[nextPointIndex] = point;
        nextPointIndex++;
    }
}
