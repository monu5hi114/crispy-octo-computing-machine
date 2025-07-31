using UnityEngine;

public static class InputHandler
{
    public static Vector2Int GetInputDirection(Vector2Int currentDir)
    {
        Vector2Int keyboardInput = GetKeyboardDirection(currentDir);
        if (keyboardInput != currentDir)
            return keyboardInput;

        Vector2Int swipeInput = GetSwipeDirection(currentDir);
        if (swipeInput != currentDir)
            return swipeInput;

        return currentDir;
    }

    public static Vector2Int GetKeyboardDirection(Vector2Int currentDir)
    {
        if (Input.GetKeyDown(KeyCode.W) && currentDir != Vector2Int.down)
            return Vector2Int.down;
        if (Input.GetKeyDown(KeyCode.S) && currentDir != Vector2Int.up)
            return Vector2Int.up;
        if (Input.GetKeyDown(KeyCode.A) && currentDir != Vector2Int.right)
            return Vector2Int.left;
        if (Input.GetKeyDown(KeyCode.D) && currentDir != Vector2Int.left)
            return Vector2Int.right;

        return currentDir;
    }

    private static Vector2Int GetSwipeDirection(Vector2Int currentDirection)
    {
        Vector2Int swipe = SwipeInputHandler.SwipeDirection;

        if (swipe != Vector2Int.zero && swipe + currentDirection != Vector2Int.zero)
        {
            SwipeInputHandler.SwipeDirection = Vector2Int.zero; // consume it
            return swipe;
        }

        SwipeInputHandler.SwipeDirection = Vector2Int.zero; // reset even if invalid
        return currentDirection;
    }
}
