using UnityEngine;
using UnityEngine.UI;

// gives every button on this canvas a click sound
// (easier than adding it to each button one by one)
public class ButtonSounds : MonoBehaviour
{
    public AudioClip clickClip;

    void Start()
    {
        foreach (Button button in GetComponentsInChildren<Button>(true))
            button.onClick.AddListener(PlayClick);
    }

    public void PlayClick()
    {
        if (SfxPlayer.Instance != null) SfxPlayer.Instance.Play(clickClip);
    }
}
