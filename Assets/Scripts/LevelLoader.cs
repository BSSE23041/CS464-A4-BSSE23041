using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// every scene change goes through here
// loads the scene in the background and shows a loading bar
public class LevelLoader : MonoBehaviour
{
    public int menuIndex = 0;
    public int creditsIndex = 6;

    public GameObject loadingPanel;
    public Image loadingBar;      // filled image, horizontal
    public TMP_Text loadingText;
    public float barSpeed = 2.5f;

    bool isLoading;

    void Awake()
    {
        loadingPanel.SetActive(false);
    }

    public void Load(int index)
    {
        if (isLoading) return; // stops double clicks loading twice
        StartCoroutine(LoadRoutine(index));
    }

    // next scene, but after credits go back to the menu
    public void LoadNext()
    {
        int current = SceneManager.GetActiveScene().buildIndex;
        int next = current >= creditsIndex ? menuIndex : current + 1;
        Load(next);
    }

    public void Reload()
    {
        Load(SceneManager.GetActiveScene().buildIndex);
    }

    public void LoadMenu()
    {
        Load(menuIndex);
    }

    IEnumerator LoadRoutine(int index)
    {
        isLoading = true;
        Time.timeScale = 1f; // timeScale doesn't reset on its own after a load

        loadingPanel.SetActive(true);
        float shown = 0f;
        SetBar(shown);

        AsyncOperation op = SceneManager.LoadSceneAsync(index);
        op.allowSceneActivation = false;

        // progress only goes up to 0.9 until we let the scene activate,
        // so divide by 0.9 to make the bar reach 100%
        while (shown < 1f)
        {
            float target = Mathf.Clamp01(op.progress / 0.9f);
            shown = Mathf.MoveTowards(shown, target, barSpeed * Time.unscaledDeltaTime);
            SetBar(shown);
            yield return null;
        }

        SetBar(1f);
        yield return new WaitForSecondsRealtime(0.15f);
        op.allowSceneActivation = true;
    }

    void SetBar(float value)
    {
        loadingBar.fillAmount = value;
        loadingText.text = "Loading... " + Mathf.RoundToInt(value * 100f) + "%";
    }
}
