using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

// Esc opens/closes the pause menu
public class PauseMenu : MonoBehaviour
{
    public static bool IsPaused { get; private set; }

    public GameObject pausePanel;
    public GameObject firstButton;
    public bool canPause = true; // set to false when the win or game over screen shows

    LevelLoader loader;

    void Awake()
    {
        loader = FindFirstObjectByType<LevelLoader>();
        pausePanel.SetActive(false);
    }

    void Update()
    {
        Keyboard keyboard = Keyboard.current;
        if (keyboard == null || !keyboard.escapeKey.wasPressedThisFrame) return;

        if (IsPaused) Resume();
        else if (canPause) Pause();
    }

    public void Pause()
    {
        IsPaused = true;
        Time.timeScale = 0f;
        AudioListener.pause = true;
        pausePanel.SetActive(true);
        SelectButton(firstButton);
    }

    public void Resume()
    {
        IsPaused = false;
        Time.timeScale = 1f;
        AudioListener.pause = false;
        pausePanel.SetActive(false);
    }

    // Resume() has to happen before loading, otherwise timeScale, AudioListener.pause
    // and IsPaused carry over and the next scene starts frozen and silent
    public void Restart()
    {
        Resume();
        loader.Reload();
    }

    public void QuitToMenu()
    {
        Resume();
        loader.LoadMenu();
    }

    // selects a button so the arrow keys work straight away
    public static void SelectButton(GameObject button)
    {
        if (EventSystem.current == null) return;
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(button);
    }
}
