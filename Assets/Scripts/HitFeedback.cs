using UnityEngine;

// listener 2 - hit sound and sparks when the player gets hurt
public class HitFeedback : MonoBehaviour
{
    public Health health;
    public ParticleSystem sparks; // just one burst, not looping
    public AudioClip hitClip;

    void OnEnable() => health.Damaged += OnHit;
    void OnDisable() => health.Damaged -= OnHit;

    void OnHit(int current, int max)
    {
        if (SfxPlayer.Instance != null) SfxPlayer.Instance.Play(hitClip);
        sparks.Play();
    }
}
