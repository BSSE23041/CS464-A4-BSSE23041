using UnityEngine;

// looping 3D sound (used for the hazard hum and the goal chime)
// spatial blend is 1 so it gets quieter the further you are
[RequireComponent(typeof(AudioSource))]
public class WorldSound : MonoBehaviour
{
    public AudioRolloffMode rolloff = AudioRolloffMode.Logarithmic;
    public float minDistance = 2f;
    public float maxDistance = 25f;

    void Awake()
    {
        AudioSource source = GetComponent<AudioSource>();
        source.spatialBlend = 1f;
        source.rolloffMode = rolloff;
        source.minDistance = minDistance;
        source.maxDistance = maxDistance;
        source.dopplerLevel = 0f; // doppler sounded weird on a looping hum
        source.loop = true;
        if (!source.isPlaying) source.Play();
    }
}
