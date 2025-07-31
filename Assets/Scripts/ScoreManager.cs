using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;

    private int score = 0;
    private bool doublePoints = false;
    


private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        UIManager.Instance.UpdateScore(score);
    }

    public void AddPoint()
    {
        score += doublePoints ? 2 : 1;
        UIManager.Instance.UpdateScore(score);
        int high = PlayerPrefs.GetInt("HighScore", 0);
        FindFirstObjectByType<SnakeController>()?.OnScoreChanged(score);
        if (score > high)
        {
            PlayerPrefs.SetInt("HighScore", score);
        }

    }

    public void EnableDoublePoints(float duration)
    {
        StopAllCoroutines(); 
        StartCoroutine(DoublePointsTimer(duration));
    }
    public void DisableDoublePoints()
    {
        doublePoints = false;
    }

    private System.Collections.IEnumerator DoublePointsTimer(float duration)
    {
        doublePoints = true;
        UIManager.Instance.ShowPowerUp("Double Points", duration);
        yield return new WaitForSeconds(duration);
        doublePoints = false;
        UIManager.Instance.HidePowerUpInfo();
    }

    public int GetScore()
    {
        return score;
    }
}
