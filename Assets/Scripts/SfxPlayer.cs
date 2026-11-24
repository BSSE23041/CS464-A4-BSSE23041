using UnityEngine;

// plays all the sound effects in the game
// it has 4 AudioSources (all going to the SFX group) so a few sounds can play at once
// it doesn't get destroyed on load so sounds don't get cut off
public class SfxPlayer : MonoBehaviour
{
    public static SfxPlayer Instance { get; private set; }

    public float pitchRange = 2f; // in semitones, up or down

    AudioSource[] sources;
    int next;

    void Awake()
    {
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        sources = GetComponents<AudioSource>();
        foreach (AudioSource s in sources)
        {
            s.playOnAwake = false;
            s.spatialBlend = 0f;
            s.ignoreListenerPause = true; // button clicks should still work when paused
        }
    }

    public void Play(AudioClip clip, float volume = 1f)
    {
        if (clip == null || sources.Length == 0) return;

        // take turns between the sources
        AudioSource source = sources[next];
        next = (next + 1) % sources.Length;

        // random pitch so the same sound doesn't get boring
        float semitones = Random.Range(-pitchRange, pitchRange);
        source.pitch = Mathf.Pow(2f, semitones / 12f);
        source.PlayOneShot(clip, volume);
    }
}
