using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [Header("Score UI")]
    public TMP_Text scoreText;
    public TMP_Text finalScoreText;
    public TMP_Text highScoreText;

    [Header("Panels")]
    public GameObject gameOverPanel;
    public GameObject pauseMenuPanel;

    [Header("Power-up UI")]
    public TMP_Text powerUpText;
    public Slider powerUpTimerSlider;

    private Coroutine powerUpSliderCoroutine;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        UpdateScore(0);
        HideGameOver();
        HidePauseMenu();
        HidePowerUpInfo();
    }

    
    public void UpdateScore(int score)
    {
        if (scoreText != null)
            scoreText.text = "Score: " + score;
    }

    
    public void ShowGameOver()
    {
        if (gameOverPanel != null)
        {
            finalScoreText.text = "Score: " + ScoreManager.Instance.GetScore();

            int current = ScoreManager.Instance.GetScore();
            int high = PlayerPrefs.GetInt("HighScore", 0);
            if (current > high)
            {
                high = current;
                PlayerPrefs.SetInt("HighScore", high);
            }

            highScoreText.text = "High Score: " + high;
            gameOverPanel.SetActive(true);
        }
    }

    public void HideGameOver()
    {
        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);
    }

    
    public void ShowPauseMenu()
    {
        if (pauseMenuPanel != null)
        {
            pauseMenuPanel.SetActive(true);
            Time.timeScale = 0f;
        }
    }

    public void HidePauseMenu()
    {
        if (pauseMenuPanel != null)
        {
            pauseMenuPanel.SetActive(false);
            Time.timeScale = 1f;
        }
    }

    public void TogglePause()
    {
        if (Time.timeScale > 0f)
            ShowPauseMenu();
        else
            HidePauseMenu();
    }

    
    public void ShowPowerUp(string name, float duration)
    {
        if (powerUpSliderCoroutine != null)
            StopCoroutine(powerUpSliderCoroutine);

        if (powerUpText != null)
        {
            powerUpText.text = name;
            powerUpText.gameObject.SetActive(true);
        }

        if (powerUpTimerSlider != null)
        {
            powerUpTimerSlider.maxValue = duration;
            powerUpTimerSlider.value = duration;
            powerUpTimerSlider.gameObject.SetActive(true);
            powerUpSliderCoroutine = StartCoroutine(UpdateSlider(duration));
        }
    }


    private System.Collections.IEnumerator UpdateSlider(float duration)
    {
        float timeLeft = duration;
        while (timeLeft > 0)
        {
            timeLeft -= Time.unscaledDeltaTime;
            powerUpTimerSlider.value = timeLeft;
            yield return null;
        }

        HidePowerUpInfo();
    }

    public void HidePowerUpInfo()
    {
        if (powerUpSliderCoroutine != null)
        {
            StopCoroutine(powerUpSliderCoroutine);
            powerUpSliderCoroutine = null;
        }

        if (powerUpText != null)
            powerUpText.gameObject.SetActive(false);

        if (powerUpTimerSlider != null)
            powerUpTimerSlider.gameObject.SetActive(false);
    }


    
    public void OnResumeButton()
    {
        HidePauseMenu();
    }

    public void OnRestartButton()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

    }

    public void OnExitButton()
    {
        SceneManager.LoadScene("MenuScene");
    }

    public void OnHomeButton()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MenuScene");
    }



}
