using UnityEngine;

/// <summary>
/// Additional functionality for GvrController.
/// </summary>
public class MyGvrController : GvrController {

    /// The percentage of the touchpad that has to be covered during a left or right swipe
    private static float swipeHorizontalThreshold = 0.6f;

    /// The percentage of the touchpad that has to be covered during a down or up swipe
    private static float swipeVerticalThreshold = 0.4f;

    /// <summary>
    /// Whether the user is swiping left (on the touchpad button) from touchOrigin. Only swipes
    /// larger than swipeHorizontalThreshold are considered.
    /// </summary>
    /// <returns><c>true</c> if TouchPos is to the left of touchOrigin beyond swipeHorizontalThreshold,
    /// <c>false</c> otherwise.</returns>
    /// <param name="touchOrigin">The beginning of the swipe to evaluate</param>
    public static bool SwipingLeftFrom (Vector2 touchOrigin)
    {
        return (touchOrigin.x - TouchPos.x) > swipeHorizontalThreshold;
    }

    /// <summary>
    /// Whether the user is swiping right (on the touchpad button) from touchOrigin. Only swipes
    /// larger than swipeHorizontalThreshold are considered.
    /// </summary>
    /// <returns><c>true</c> if TouchPos is to the right of touchOrigin beyond swipeHorizontalThreshold,
    /// <c>false</c> otherwise.</returns>
    /// <param name="touchOrigin">The beginning of the swipe to evaluate</param>
    public static bool SwipingRightFrom (Vector2 touchOrigin)
    {
        return (TouchPos.x - touchOrigin.x) > swipeHorizontalThreshold;
    }

    /// <summary>
    /// Whether the user is swiping down (on the touchpad button) from touchOrigin. Only swipes
    /// larger than swipeVerticalThreshold are considered.
    /// </summary>
    /// <returns><c>true</c> if TouchPos is below touchOrigin beyond swipeVerticalThreshold, <c>false</c>
    /// otherwise.</returns>
    /// <param name="touchOrigin">The beginning of the swipe to evaluate</param>
    public static bool SwipingDownFrom (Vector2 touchOrigin)
    {
        return (TouchPos.y - touchOrigin.y) > swipeVerticalThreshold;
    }

    /// <summary>
    /// Whether the user is swiping up (on the touchpad button) from touchOrigin. Only swipes
    /// larger than swipeVerticalThreshold are considered.
    /// </summary>
    /// <returns><c>true</c> if TouchPos is above touchOrigin beyond swipeVerticalThreshold, <c>false</c>
    /// otherwise.</returns>
    /// <param name="touchOrigin">The beginning of the swipe to evaluate</param>
    public static bool SwipingUpFrom (Vector2 touchOrigin)
    {
        return (touchOrigin.y - TouchPos.y) > swipeVerticalThreshold;
    }
}
