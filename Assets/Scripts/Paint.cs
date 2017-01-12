using UnityEngine;

/// <summary>
/// Encapsulates paint colors and keeps track of the active one.
/// </summary>
public static class Paint {

    // TODO: make both these properties private after simple save/load testing
    public static Color[] Colors = {
        Color.white,
        Color.gray,
        Color.black,
        Color.red,
        Color.green,
        Color.blue,
        Color.cyan,
        Color.magenta,
        Color.yellow
    };
    public static int currentPaintIndex = -1;

    public static Color NextColor ()
    {
        currentPaintIndex = (currentPaintIndex + 1) % Colors.Length;
        return Colors[currentPaintIndex];
    }

    public static Color PreviousColor ()
    {
        currentPaintIndex = (currentPaintIndex - 1 + Colors.Length) % Colors.Length;
        return Colors[currentPaintIndex];
    }
}
