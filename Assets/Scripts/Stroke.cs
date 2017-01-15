using UnityEngine;
using System.Collections.Generic;

public class Stroke {

    private static int maxPoints = 100; // TODO: find good number for this
    private static float strokeOffsetZ = -0.5f;

    /// Must match Trail Renderer Min Vertex Distance, which is not scriptable :(
    private static float pointPrecision = 0.001f;

    private Transform canvas;
    private Transform pencil;
    private GameObject paint;
    private GameObject loadingPaint;
    private List<StrokeData> strokeDataList;
    private StrokeData currentStrokeData;
    private int currentStrokeDataIndex;
    private int nextPointIndex;
    private int pointCount;

    public Stroke (Transform canvas, Transform pencil, GameObject paint)
    {
        this.canvas = canvas;
        this.pencil = pencil;

        this.paint = GameObject.Instantiate<GameObject>(paint);

        Color color = paint.gameObject.GetComponent<TrailRenderer>().material.GetColor("_EmissionColor");
        strokeDataList = new List<StrokeData>();
        currentStrokeData = new StrokeData(color, maxPoints);
        strokeDataList.Add(currentStrokeData);

        currentStrokeDataIndex = 0;
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
            nextPointIndex = 0;
            currentStrokeDataIndex++;
            currentStrokeData = new StrokeData(currentStrokeData.color, maxPoints);
            strokeDataList.Add(currentStrokeData);
        }

        Vector3 currentPoint = paint.transform.position;
        if (nextPointIndex == 0 || AreDistinctEnough(currentPoint, currentStrokeData.points[nextPointIndex - 1])) {
            currentStrokeData.points[nextPointIndex] = currentPoint;
            nextPointIndex++;
            pointCount++;
        }
    }

    public void StartLoading (GameObject loadingPencil)
    {
        currentStrokeDataIndex = 0;
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
        if (nextPointIndex == 0 || nextPointIndex > maxPoints - 1) {
            nextPointIndex = 0;
            currentStrokeData = strokeDataList[currentStrokeDataIndex++];
        }
        return currentStrokeData.points[nextPointIndex++];
    }

    public bool HasMorePoints ()
    {
        return currentStrokeDataIndex < strokeDataList.Count || nextPointIndex < pointCount % maxPoints;
    }

    private bool AreDistinctEnough (Vector3 v1, Vector3 v2)
    {
        return (v1 - v2).sqrMagnitude > (v1 * pointPrecision).sqrMagnitude;
    }
}
