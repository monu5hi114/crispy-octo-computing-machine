using UnityEngine;

public static class InputHandler
{
    public static Vector2Int GetInputDirection(Vector2Int currentDir)
    {
        if (Input.GetKeyDown(KeyCode.W) && currentDir != Vector2Int.down && currentDir != Vector2Int.up)
            return Vector2Int.down;
        if (Input.GetKeyDown(KeyCode.S) && currentDir != Vector2Int.up && currentDir != Vector2Int.down)
            return Vector2Int.up;
        if (Input.GetKeyDown(KeyCode.A) && currentDir != Vector2Int.right && currentDir != Vector2Int.left)
            return Vector2Int.left;
        if (Input.GetKeyDown(KeyCode.D) && currentDir != Vector2Int.left && currentDir != Vector2Int.right)
            return Vector2Int.right;

        return currentDir;
    }
}
