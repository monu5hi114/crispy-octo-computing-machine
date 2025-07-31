using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class FoodSpawner
{
    public static void Spawn(ref Vector2Int foodPos, List<Vector2Int> snake)
    {
        List<Vector2Int> empty = new List<Vector2Int>();
        GameObject[,] grid = GridManager.Instance.grid;

        for (int x = 0; x < 20; x++)
        {
            for (int y = 0; y < 20; y++)
            {
                Vector2Int pos = new Vector2Int(x, y);
                if (!snake.Contains(pos))
                    empty.Add(pos);
            }
        }

        if (empty.Count == 0)
        {
            Debug.Log("You Win!");
            return;
        }

        foodPos = empty[Random.Range(0, empty.Count)];
        GameObject cell = grid[foodPos.x, foodPos.y];

        cell.GetComponent<Image>().enabled = false;
        cell.transform.GetChild(0).gameObject.SetActive(false);
        cell.transform.GetChild(2).gameObject.SetActive(true); // sanp ka khana
    }
}
