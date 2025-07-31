using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GridSpawner : MonoBehaviour
{
    public GameObject cellPrefab; 
    private float timer = 0f;
    public float interval = 1f;
    public GameObject[,] arr = new GameObject[20, 20];
    List<Vector2Int> snake = new List<Vector2Int>();
    private Vector2Int direction = Vector2Int.right;

    private Vector2Int foodPosition;
    private bool grow = false;

    private void Awake()
    {
        for (int x = 0; x < 20; x++)
        {
            for (int y = 0; y < 20; y++)
            {
                GameObject cell = Instantiate(cellPrefab);
                arr[x, y] = cell;
                cell.transform.SetParent(transform);

                var img = cell.GetComponent<Image>();
                img.enabled = false;

                cell.transform.GetChild(0).gameObject.SetActive(false); 
                cell.transform.GetChild(1).gameObject.SetActive(false); 
            }
        }
    }

    IEnumerator Start()
    {
        yield return null;
        GetComponent<GridLayoutGroup>().enabled = false;

        Vector2Int start = new Vector2Int(10, 10);
        snake.Add(start);
        var cell = arr[start.x, start.y];
        cell.GetComponent<Image>().enabled = true;
        cell.transform.GetChild(0).gameObject.SetActive(true); 

        SpawnFood();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W) && direction != Vector2Int.down && direction != Vector2Int.up)
            direction = Vector2Int.down;
        if (Input.GetKeyDown(KeyCode.S) && direction != Vector2Int.down && direction != Vector2Int.up)
            direction = Vector2Int.up;
        if (Input.GetKeyDown(KeyCode.A) && direction != Vector2Int.right && direction != Vector2Int.left)
            direction = Vector2Int.left;
        if (Input.GetKeyDown(KeyCode.D) && direction != Vector2Int.right && direction != Vector2Int.left)
            direction = Vector2Int.right;
    }

    void FixedUpdate()
    {
        timer += Time.fixedDeltaTime;
        if (timer >= interval)
        {
            Vector2Int head = snake[0];
            Vector2Int newHead = head + direction;

            if (newHead.x < 0 || newHead.x >= 20 || newHead.y < 0 || newHead.y >= 20)
            {
                Debug.Log("Game Over: Hit the wall!");
                return;
            }

            if (snake.Contains(newHead))
            {
                Debug.Log("Game Over: Hit yourself!");
                return;
            }

            snake.Insert(0, newHead);
            GameObject newHeadCell = arr[newHead.x, newHead.y];

            newHeadCell.GetComponent<Image>().enabled = true;
            newHeadCell.transform.GetChild(0).gameObject.SetActive(true); 
            newHeadCell.transform.GetChild(1).gameObject.SetActive(false); 

            Transform headTransform = newHeadCell.transform.GetChild(0);
            headTransform.rotation = Quaternion.Euler(0, 0, GetRotationAngle(direction));

            if (snake.Count > 1)
            {
                Vector2Int prevHead = snake[1];
                GameObject prevCell = arr[prevHead.x, prevHead.y];
                prevCell.transform.GetChild(0).gameObject.SetActive(false); 
            }

            if (newHead == foodPosition)
            {
                grow = true;
                newHeadCell.transform.GetChild(2).gameObject.SetActive(false); 
                SpawnFood();
            }

            if (!grow)
            {
                Vector2Int tail = snake[snake.Count - 1];
                GameObject tailCell = arr[tail.x, tail.y];
                tailCell.GetComponent<Image>().enabled = false;
                tailCell.transform.GetChild(0).gameObject.SetActive(false); 
                tailCell.transform.GetChild(1).gameObject.SetActive(false); 
                snake.RemoveAt(snake.Count - 1);
            }
            else
            {
                grow = false;
            }

            if (snake.Count > 1)
            {
                Vector2Int newTail = snake[snake.Count - 1];
                Vector2Int beforeTail = snake[snake.Count - 2];
                Vector2Int tailDirection = newTail - beforeTail;

                GameObject tailCell = arr[newTail.x, newTail.y];
                tailCell.transform.GetChild(1).gameObject.SetActive(true); 

                Transform tailTransform = tailCell.transform.GetChild(1);
                tailTransform.rotation = Quaternion.Euler(0, 0, GetRotationAngle(tailDirection));
            }

            timer = 0f;
        }
    }
    private float GetRotationAngle(Vector2Int dir)
    {
        if (dir == Vector2Int.up) return 180f;
        if (dir == Vector2Int.right) return -90f;
        if (dir == Vector2Int.down) return 0f;
        if (dir == Vector2Int.left) return 90f;
        return 0f;
    }


    void SpawnFood()
    {
        List<Vector2Int> empty = new List<Vector2Int>();

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

        foodPosition = empty[Random.Range(0, empty.Count)];

        GameObject foodCell = arr[foodPosition.x, foodPosition.y];
        foodCell.GetComponent<Image>().enabled = false;
        foodCell.transform.GetChild(0).gameObject.SetActive(false); 
        foodCell.transform.GetChild(2).gameObject.SetActive(true);  
    }
}
