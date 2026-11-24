using UnityEngine;

// purple blocks that hurt the player
// some go up and down (bob) and the long bars spin
public class Hazard : MonoBehaviour
{
    public int damage = 1;

    [Header("Movement")]
    public bool bob = false;
    public float bobHeight = 2.6f; // high enough for the player to run under
    public float bobSpeed = 2f;
    public float phase = 0f;       // so they don't all move at the same time
    public float spinSpeed = 0f;

    Vector3 startPosition;

    void Start()
    {
        startPosition = transform.position;
    }

    void Update()
    {
        if (bob)
        {
            float t = Mathf.Sin(Time.time * bobSpeed + phase) * 0.5f + 0.5f;
            transform.position = startPosition + Vector3.up * (t * bobHeight);
        }

        if (spinSpeed != 0f)
            transform.Rotate(0f, spinSpeed * Time.deltaTime, 0f);
    }

    void OnTriggerStay(Collider other)
    {
        Health health = other.GetComponent<Health>();
        if (health != null) health.TakeDamage(damage);
    }
}
