using UnityEngine;

// credits scene
// the back button uses LoadNext() which goes from credits back to the menu
public class CreditsMenu : MonoBehaviour
{
    public LevelLoader loader;
    public GameObject backButton;

    void Start()
    {
        PauseMenu.SelectButton(backButton);
    }

    public void BackToMenu()
    {
        loader.LoadNext();
    }
}
