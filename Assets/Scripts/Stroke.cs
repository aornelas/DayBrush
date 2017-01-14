using UnityEngine;
using System.Collections;

public class Stroke {

    private static int maxPoints = 1000; // TODO: find good number for this
    private static float strokeOffsetZ = -0.5f;

    /// Must match Trail Renderer Min Vertex Distance, which is not scriptable :(
    private static float pointPrecision = 0.001f;

    private Transform canvas;
    private Transform pencil;
    private GameObject paint;
    private GameObject loadingPaint;
    private Vector3[] points;
    private int nextPointIndex;
    private int pointCount;

    public Stroke (Transform canvas, Transform pencil, GameObject paint)
    {
        this.canvas = canvas;
        this.pencil = pencil;

        this.paint = GameObject.Instantiate<GameObject>(paint);
//        pointPrecision = this.paint.GetComponent<TrailRenderer>().

        points = new Vector3[maxPoints];
        nextPointIndex = 0;
        pointCount = 0;
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

    public void RecordPoint ()
    {
        if (nextPointIndex > maxPoints - 1) {
            // TODO: stop recording and start new stroke
            return;
        }

        Vector3 currentPoint = paint.transform.position;
        if (nextPointIndex == 0 || AreDistinctEnough(currentPoint, points[nextPointIndex - 1])) {
            points[nextPointIndex] = currentPoint;
//            Debug.Log("points[" + nextPointIndex + "] = " + points[nextPointIndex]);
            nextPointIndex++;
            pointCount++;
        }
    }

    public void StartLoading (GameObject loadingPencil)
    {
        nextPointIndex = 0;
        loadingPaint = GameObject.Instantiate<GameObject>(paint);
        Vector3 startingPoint = NextPoint();
        loadingPencil.gameObject.GetComponent<MeshRenderer>().enabled = false;
        loadingPencil.transform.position = startingPoint;
        loadingPaint.transform.position = startingPoint;
        loadingPaint.transform.SetParent(loadingPencil.transform);
        loadingPaint.gameObject.GetComponent<TrailRenderer>().enabled = true;
        loadingPaint.SetActive(true);
    }

    public void FinishLoading ()
    {
        loadingPaint.SetActive(false);
        Show();
    }

    public Vector3 NextPoint ()
    {
        return points[nextPointIndex++];
    }

    public bool HasMorePoints ()
    {
        return nextPointIndex < pointCount;
    }

    private bool AreDistinctEnough (Vector3 v1, Vector3 v2)
    {
        return (v1 - v2).sqrMagnitude > (v1 * pointPrecision).sqrMagnitude;
    }
}
