using UnityEngine;

// coin pickup
// the sound is played by the SfxPlayer and not by the coin, otherwise
// it would get cut off when the coin is destroyed
public class Pickup : MonoBehaviour
{
    public int points = 10;
    public AudioClip pickupClip;
    public GameObject effectPrefab; // particles, stop action is set to Destroy
    public float spinSpeed = 120f;

    bool taken;

    void Update()
    {
        transform.Rotate(0f, spinSpeed * Time.deltaTime, 0f, Space.World);
    }

    void OnTriggerEnter(Collider other)
    {
        if (taken || !other.CompareTag("Player")) return;
        taken = true;

        if (SfxPlayer.Instance != null) SfxPlayer.Instance.Play(pickupClip);
        if (effectPrefab != null) Instantiate(effectPrefab, transform.position, Quaternion.identity);
        if (LevelManager.Instance != null) LevelManager.Instance.AddScore(points);

        Destroy(gameObject);
    }
}
