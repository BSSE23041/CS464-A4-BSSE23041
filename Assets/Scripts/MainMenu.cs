using UnityEngine;
using UnityEngine.InputSystem;

// main menu buttons
public class MainMenu : MonoBehaviour
{
    public LevelLoader loader;
    public GameObject mainPanel;
    public GameObject firstMainButton;
    public GameObject levelSelectPanel;
    public LevelSelect levelSelect;

    void Start()
    {
        ShowMain();
    }

    void Update()
    {
        // Esc also goes back from the level select
        Keyboard keyboard = Keyboard.current;
        if (keyboard != null && keyboard.escapeKey.wasPressedThisFrame && levelSelectPanel.activeSelf)
            ShowMain();
    }

    public void ShowMain()
    {
        levelSelectPanel.SetActive(false);
        mainPanel.SetActive(true);
        PauseMenu.SelectButton(firstMainButton);
    }

    public void OpenLevelSelect()
    {
        mainPanel.SetActive(false);
        levelSelectPanel.SetActive(true);
        levelSelect.Refresh(); // update locks and best times every time it opens
        PauseMenu.SelectButton(levelSelect.FirstButton());
    }

    public void Play()
    {
        loader.Load(1);
    }

    public void OpenCredits()
    {
        loader.Load(loader.creditsIndex);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
