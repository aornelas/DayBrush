using UnityEngine;
using System.Collections;

/// <summary>
/// Represents a stroke to be saved and loaded within PaintingData.
/// </summary>
[System.Serializable]
// TODO: rename to StrokeSegment
public class StrokeData {

    public Vector3[] points;
    public int pointCount;
    public Color color;

    public StrokeData (Color color, int maxPoints)
    {
        this.color = color;
        points = new Vector3[maxPoints];
    }

}

