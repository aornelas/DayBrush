using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Detects gestures done on the touchpad of the GvrController
/// </summary>
public class GvrControllerGesture {

    /// The percentage of the touchpad that has to be covered during a left or right swipe
    private static float swipeHorizontalThreshold = 0.6f;

    /// The percentage of the touchpad that has to be covered during a down or up swipe
    private static float swipeVerticalThreshold = 0.3f;
    
    /// The position of the beginning of the gesture (when this class was initialized)
    private Vector2 startPosition;

    /// <summary>
    /// Initializes the GvrControllerGesture class, marking the beginning of the gesture.
    /// Usually called within Update() when GvrController.TouchDown is true.
    /// </summary>
    public GvrControllerGesture ()
    {
        this.startPosition = GvrController.TouchPos;
    }

    /// <summary>
    /// Whether the user swiped left. Only swipes larger than swipeHorizontalThreshold are considered.
    /// </summary>
    /// <returns><c>true</c>, if the current touch position is to the left of the beginning of the gesture beyond
    /// swipeHorizontalThreshold, <c>false</c> otherwise.</returns>
    public bool SwipedLeft ()
    {
        return (startPosition.x - GvrController.TouchPos.x) > swipeHorizontalThreshold;
    }

    /// <summary>
    /// Whether the user swiped right. Only swipes larger than swipeHorizontalThreshold are considered.
    /// </summary>
    /// <returns><c>true</c>, if the current touch position is to the right of the beginning of the gesture beyond
    /// swipeHorizontalThreshold, <c>false</c> otherwise.</returns>
    public bool SwipedRight ()
    {
        return (GvrController.TouchPos.x - startPosition.x) > swipeHorizontalThreshold;
    }

    /// <summary>
    /// Whether the user swiped up. Only swipes larger than swipeVerticalThreshold are considered.
    /// </summary>
    /// <returns><c>true</c>, if the current touch position is above the beginning of the gesture beyond
    /// swipeVerticalThreshold, <c>false</c> otherwise.</returns>
    public bool SwipedUp ()
    {
        return (startPosition.y - GvrController.TouchPos.y) > swipeVerticalThreshold;
    }

    /// <summary>
    /// Whether the user swiped down. Only swipes larger than swipeVerticalThreshold are considered.
    /// </summary>
    /// <returns><c>true</c>, if the current touch position is below the beginning of the gesture beyond
    /// swipeVerticalThreshold, <c>false</c> otherwise.</returns>
    public bool SwipedDown ()
    {
        return (GvrController.TouchPos.y - startPosition.y) > swipeVerticalThreshold;
    }

}
