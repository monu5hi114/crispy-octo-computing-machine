using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PowerUpManager : MonoBehaviour
{
    public static PowerUpManager Instance;

    private SnakeController snake;

    private void Awake()
    {
        Instance = this;
    }

    public void Init(SnakeController controller)
    {
        snake = controller;
        StartCoroutine(SpawnPowerUps());
    }

    public IEnumerator SpawnPowerUps()
    {
       yield return new WaitForSeconds(Random.Range(5f, 15f)); // koi bhi superpower spawn ho jayegi
       
    }

    void SpawnRandomPowerUp()
    {
        List<Vector2Int> empty = new List<Vector2Int>();
        var grid = GridManager.Instance.grid;
        var snakeBody = snake.GetSnakeBody();

        for (int x = 0; x < 20; x++)
        {
            for (int y = 0; y < 20; y++)
            {
                Vector2Int pos = new Vector2Int(x, y);
                if (!snakeBody.Contains(pos))
                    empty.Add(pos);
            }
        }

        if (empty.Count == 0) return;

        Vector2Int posToSpawn = empty[Random.Range(0, empty.Count)];
        GameObject cell = grid[posToSpawn.x, posToSpawn.y];
        cell.GetComponent<Image>().enabled = false;

        int rand = Random.Range(0, 4); 
        Transform icon = cell.transform.GetChild(3 + rand); 
        icon.gameObject.SetActive(true);
        snake.RegisterPowerUp(posToSpawn, rand);
    }
}
