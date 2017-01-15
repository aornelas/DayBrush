using UnityEngine;
using System.Collections;

/// <summary>
/// Represents a stroke to be saved and loaded within PaintingData.
/// </summary>
[System.Serializable]
public class StrokeData {

    public Vector3[] points;
    public Color color;

    public StrokeData (Color color, int maxPoints)
    {
        this.color = color;
        points = new Vector3[maxPoints];
    }

}

