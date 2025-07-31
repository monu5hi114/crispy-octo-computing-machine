using UnityEngine;

public class SwipeInputHandler : MonoBehaviour
{
    public static Vector2Int SwipeDirection = Vector2Int.right;

    private Vector2 startPosition;
    private Vector2 endPosition;

    void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
                startPosition = touch.position;

            else if (touch.phase == TouchPhase.Ended)
            {
                endPosition = touch.position;
                DetectSwipe();
            }
        }
        else if (Input.GetMouseButtonDown(0))
        {
            startPosition = Input.mousePosition;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            endPosition = Input.mousePosition;
            DetectSwipe();
        }
    }

    private void DetectSwipe()
    {
        Vector2 delta = endPosition - startPosition;

        if (delta.magnitude > 50f)
        {
            if (Mathf.Abs(delta.x) > Mathf.Abs(delta.y))
                SwipeDirection = delta.x > 0 ? Vector2Int.right : Vector2Int.left;
            else
                SwipeDirection = delta.y > 0 ? Vector2Int.down : Vector2Int.up;

            Debug.Log("Swipe: " + SwipeDirection);
        }
    }
}
