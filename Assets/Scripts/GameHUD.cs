using TMPro;
using UnityEngine;

// HUD prefab that goes in all 5 levels
// has the score/lives/time texts and the win and game over screens
public class GameHUD : MonoBehaviour
{
    [Header("HUD texts")]
    public TMP_Text levelText;
    public TMP_Text scoreText;
    public TMP_Text livesText;
    public TMP_Text timeText;

    [Header("Win screen")]
    public GameObject winPanel;
    public GameObject winFirstButton;
    public TMP_Text winInfoText;
    public TMP_Text nextButtonText;

    [Header("Game over screen")]
    public GameObject gameOverPanel;
    public GameObject gameOverFirstButton;

    public PauseMenu pauseMenu;

    LevelLoader loader;

    void Awake()
    {
        loader = FindFirstObjectByType<LevelLoader>();
        winPanel.SetActive(false);
        gameOverPanel.SetActive(false);
    }

    public void SetLevel(int level) { levelText.text = "Level " + level; }
    public void SetScore(int score) { scoreText.text = "Score: " + score; }
    public void SetLives(int lives) { livesText.text = "Lives: " + lives; }
    public void SetTime(float time) { timeText.text = "Time: " + time.ToString("0.0") + " s"; }

    public void ShowWinScreen(float time, bool newBest, float bestTime, bool lastLevel)
    {
        pauseMenu.canPause = false;
        winPanel.SetActive(true);

        string info = "Your time: " + time.ToString("0.0") + " s\n";
        if (newBest) info += "New best time!";
        else info += "Best time: " + bestTime.ToString("0.0") + " s";
        winInfoText.text = info;

        // after level 5 the next scene is the credits
        nextButtonText.text = lastLevel ? "Credits" : "Next level";
        PauseMenu.SelectButton(winFirstButton);
    }

    public void ShowGameOver()
    {
        pauseMenu.canPause = false;
        gameOverPanel.SetActive(true);
        PauseMenu.SelectButton(gameOverFirstButton);
    }

    // buttons
    public void NextLevel() { loader.LoadNext(); }
    public void RestartLevel() { loader.Reload(); }
    public void MainMenu() { loader.LoadMenu(); }
}
