using UnityEngine;

// one in every level. keeps track of score, lives and the timer
// and decides when you win or lose
// listener 4 - listens for the player dying
public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { get; private set; }

    public GameHUD hud;
    public Health playerHealth;
    public PlayerController player;
    public int startLives = 3;

    public AudioClip winClip;
    public AudioClip loseClip;

    int score;
    int lives;
    bool finished;

    void Awake()
    {
        Instance = this;
        lives = startLives;
    }

    void OnDestroy()
    {
        if (Instance == this) Instance = null;
    }

    void OnEnable() => playerHealth.Died += OnPlayerDied;
    void OnDisable() => playerHealth.Died -= OnPlayerDied;

    void Start()
    {
        hud.SetLevel(gameObject.scene.buildIndex);
        hud.SetScore(score);
        hud.SetLives(lives);
        hud.SetTime(0f);
    }

    void Update()
    {
        if (!finished) hud.SetTime(Time.timeSinceLevelLoad);
    }

    public void AddScore(int points)
    {
        score += points;
        hud.SetScore(score);
    }

    void OnPlayerDied()
    {
        lives--;
        hud.SetLives(lives);

        if (lives > 0)
        {
            // back to the start with full health
            player.Respawn();
            playerHealth.ResetHealth();
        }
        else
        {
            GameOver();
        }
    }

    void GameOver()
    {
        if (finished) return;
        finished = true;
        player.SetInputEnabled(false);
        if (SfxPlayer.Instance != null) SfxPlayer.Instance.Play(loseClip);
        hud.ShowGameOver();
    }

    // called by the Goal. saves the time, unlocks the next level and shows the win screen
    public void Won()
    {
        if (finished) return;
        finished = true;

        int level = gameObject.scene.buildIndex;
        float time = Time.timeSinceLevelLoad;
        bool best = SaveData.Record(level, time);
        SaveData.Unlock(level + 1);

        player.SetInputEnabled(false);
        if (SfxPlayer.Instance != null) SfxPlayer.Instance.Play(winClip);
        hud.SetTime(time);
        hud.ShowWinScreen(time, best, SaveData.GetBest(level), level >= SaveData.LevelCount);
    }
}
