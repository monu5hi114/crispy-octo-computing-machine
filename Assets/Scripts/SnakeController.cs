using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SnakeController : MonoBehaviour
{
    public float interval = 1f;
    private float baseInterval;
    private float timer = 0f;

    private List<Vector2Int> snake = new List<Vector2Int>();
    private Vector2Int direction = Vector2Int.right;
    private Vector2Int foodPosition;
    private bool grow = false;

    private Dictionary<Vector2Int, int> powerUps = new Dictionary<Vector2Int, int>();

    private Coroutine activePowerUpCoroutine = null;
    private int currentPowerUp = -1;

    private void Start()
    {
        StartCoroutine(Init());
    }

    private IEnumerator Init()
    {
        yield return null;
        GetComponent<GridLayoutGroup>().enabled = false;

        Vector2Int start = new Vector2Int(10, 10);
        snake.Add(start);

        var cell = GridManager.Instance.grid[start.x, start.y];
        cell.GetComponent<Image>().enabled = true;
        cell.transform.GetChild(0).gameObject.SetActive(true);

        baseInterval = interval;
        FoodSpawner.Spawn(ref foodPosition, snake);
        PowerUpManager.Instance.Init(this);
    }

    private void Update()
    {
        direction = InputHandler.GetInputDirection(direction);
    }

    private void FixedUpdate()
    {
        timer += Time.fixedDeltaTime;
        if (timer >= interval)
        {
            Step();
            timer = 0f;
        }
    }

    private void Step()
    {
        Vector2Int newHead = snake[0] + direction;

        if (newHead.x < 0 || newHead.x >= 20 || newHead.y < 0 || newHead.y >= 20 || snake.Contains(newHead))
        {
            UIManager.Instance.ShowGameOver();
            AudioManager.Instance.PlayWallHitSound();
            enabled = false;
            return;
        }

        snake.Insert(0, newHead);
        UpdateVisuals(newHead);

        if (newHead == foodPosition)
        {
            grow = true;
            GridManager.Instance.grid[newHead.x, newHead.y].transform.GetChild(2).gameObject.SetActive(false);
            FoodSpawner.Spawn(ref foodPosition, snake);
            ScoreManager.Instance.AddPoint();
            AudioManager.Instance.PlayEatSound();
        }

        if (powerUps.ContainsKey(newHead))
        {
            int type = powerUps[newHead];
            ApplyPowerUp(type);
            GridManager.Instance.grid[newHead.x, newHead.y].transform.GetChild(3 + type).gameObject.SetActive(false);
            powerUps.Remove(newHead);
        }

        if (!grow)
        {
            Vector2Int tail = snake[snake.Count - 1];
            GameObject tailCell = GridManager.Instance.grid[tail.x, tail.y];
            tailCell.GetComponent<Image>().enabled = false;
            tailCell.transform.GetChild(0).gameObject.SetActive(false);
            tailCell.transform.GetChild(1).gameObject.SetActive(false);
            snake.RemoveAt(snake.Count - 1);
        }
        else
        {
            grow = false;
        }

        UpdateTailVisuals();
    }

    private void UpdateVisuals(Vector2Int headPos)
    {
        GameObject cell = GridManager.Instance.grid[headPos.x, headPos.y];
        cell.GetComponent<Image>().enabled = true;

        GameObject head = cell.transform.GetChild(0).gameObject;
        head.SetActive(true);
        head.transform.rotation = Quaternion.Euler(0, 0, DirectionHelper.GetRotationAngle(direction));

        if (snake.Count > 1)
        {
            Vector2Int prevHead = snake[1];
            GridManager.Instance.grid[prevHead.x, prevHead.y].transform.GetChild(0).gameObject.SetActive(false);
        }
    }

    private void UpdateTailVisuals()
    {
        if (snake.Count > 1)
        {
            Vector2Int tail = snake[snake.Count - 1];
            Vector2Int beforeTail = snake[snake.Count - 2];
            Vector2Int tailDir = tail - beforeTail;

            GameObject tailCell = GridManager.Instance.grid[tail.x, tail.y];
            GameObject tailObj = tailCell.transform.GetChild(1).gameObject;

            tailObj.SetActive(true);
            tailObj.transform.rotation = Quaternion.Euler(0, 0, DirectionHelper.GetRotationAngle(tailDir));
        }
    }

    public void RegisterPowerUp(Vector2Int pos, int type)
    {
        if (!powerUps.ContainsKey(pos))
        {
            powerUps.Add(pos, type);
        }
    }

    public List<Vector2Int> GetSnakeBody()
    {
        return snake;
    }

    private void ApplyPowerUp(int type)
    {
        PowerUpManager.Instance.StartCoroutine(PowerUpManager.Instance.SpawnPowerUps());
        if (activePowerUpCoroutine != null)
        {
            StopCoroutine(activePowerUpCoroutine);
            CancelCurrentPowerUpEffect(currentPowerUp);
        }

        currentPowerUp = type;

        switch (type)
        {
            case 0:
                interval = baseInterval / 2f;
                UIManager.Instance.ShowPowerUp("Speed Boost", 5f);
                AudioManager.Instance.SetHighSpeed();
                activePowerUpCoroutine = StartCoroutine(ResetSpeedAfter(5f));
                break;

            case 1:
                interval = baseInterval * 2f;
                UIManager.Instance.ShowPowerUp("Slow Motion", 5f);
                AudioManager.Instance.SetSlowMotion();
                activePowerUpCoroutine = StartCoroutine(ResetSpeedAfter(5f));
                break;

            case 2:
                int segmentsToTrim = Mathf.Min(5, snake.Count - 2);

                for (int i = 0; i < segmentsToTrim; i++)
                {
                    Vector2Int tail = snake[snake.Count - 1];
                    GameObject tailCell = GridManager.Instance.grid[tail.x, tail.y];

                    tailCell.GetComponent<Image>().enabled = false;
                    tailCell.transform.GetChild(0).gameObject.SetActive(false);
                    tailCell.transform.GetChild(1).gameObject.SetActive(false);

                    snake.RemoveAt(snake.Count - 1);
                }

                UIManager.Instance.ShowPowerUp("Trim Snake", 2f);
                AudioManager.Instance.PlayTrimSound();
                activePowerUpCoroutine = StartCoroutine(HideTrimInfoAfter(2f));
                break;

            case 3:
                ScoreManager.Instance.EnableDoublePoints(10f);
                UIManager.Instance.ShowPowerUp("Double Points", 10f);
                AudioManager.Instance.PlayDoubleScoreSound();
                activePowerUpCoroutine = StartCoroutine(ResetDoublePointsAfter(10f));
                break;
        }
    }

    private void CancelCurrentPowerUpEffect(int type)
    {
        switch (type)
        {
            case 0:
            case 1:
                interval = baseInterval;
                AudioManager.Instance.SetNormalSpeed();
                break;

            case 3:
                ScoreManager.Instance.DisableDoublePoints();
                break;
        }

        UIManager.Instance.HidePowerUpInfo();
    }

    private IEnumerator ResetSpeedAfter(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        interval = baseInterval;
        AudioManager.Instance.SetNormalSpeed();
        UIManager.Instance.HidePowerUpInfo();
        activePowerUpCoroutine = null;
        currentPowerUp = -1;
    }

    private IEnumerator ResetDoublePointsAfter(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        ScoreManager.Instance.DisableDoublePoints();
        UIManager.Instance.HidePowerUpInfo();
        activePowerUpCoroutine = null;
        currentPowerUp = -1;
    }

    private IEnumerator HideTrimInfoAfter(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        UIManager.Instance.HidePowerUpInfo();
        activePowerUpCoroutine = null;
        currentPowerUp = -1;
    }

    public void OnScoreChanged(int score)
    {
        float minInterval = 0.1f;
        float maxSpeedScore = 50f;
        float t = Mathf.Clamp01(score / maxSpeedScore);
        interval = Mathf.Lerp(baseInterval, minInterval, t);
    }
}
