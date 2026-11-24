using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

// this is on the Canvas and not on the pause panel, because the pause panel
// is hidden at the start and its scripts wouldn't run until it opens
public class VolumeSlider : MonoBehaviour
{
    public AudioMixer mixer;
    public Slider slider;
    public string parameter = "MusicVolume";

    void Start()
    {
        slider.minValue = 0.0001f; // not 0 because Log10(0) breaks
        slider.maxValue = 1f;

        float saved = SaveData.GetMusicVolume();
        slider.SetValueWithoutNotify(saved);
        Apply(saved);

        slider.onValueChanged.AddListener(OnSliderChanged);
    }

    void OnDestroy()
    {
        if (slider != null) slider.onValueChanged.RemoveListener(OnSliderChanged);
    }

    void OnSliderChanged(float value)
    {
        Apply(value);
        SaveData.SetMusicVolume(value);
    }

    void Apply(float value)
    {
        // slider value to decibels: 1 = 0 dB, 0.5 = -6 dB, 0.0001 = -80 dB
        mixer.SetFloat(parameter, Mathf.Log10(value) * 20f);
    }
}
