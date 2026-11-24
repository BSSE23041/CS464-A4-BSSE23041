using UnityEngine;

// music that keeps playing between scenes
// every scene has one of these so I can test any scene on its own,
// but only the first one stays
[RequireComponent(typeof(AudioSource))]
public class PersistentMusic : MonoBehaviour
{
    static PersistentMusic instance;

    void Awake()
    {
        if (instance != null) { Destroy(gameObject); return; }
        instance = this;
        DontDestroyOnLoad(gameObject);

        AudioSource source = GetComponent<AudioSource>();
        source.loop = true;
        source.ignoreListenerPause = true; // so you can still hear it when changing volume in the pause menu
        if (!source.isPlaying) source.Play();
    }
}
