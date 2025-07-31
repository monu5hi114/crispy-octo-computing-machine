using UnityEngine;

public static class DirectionHelper
{
    public static float GetRotationAngle(Vector2Int dir)
    {
        if (dir == Vector2Int.up) return 180f;
        if (dir == Vector2Int.right) return -90f;
        if (dir == Vector2Int.down) return 0f;
        if (dir == Vector2Int.left) return 90f;
        return 0f;
    }
}
