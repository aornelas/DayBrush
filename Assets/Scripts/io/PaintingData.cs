using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Represents a painting to be saved and loaded.
/// </summary>
[System.Serializable]
public class PaintingData {

    public string name;
    public List<List<StrokeData>> strokes;

}
