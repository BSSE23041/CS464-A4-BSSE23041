using System;
using UnityEngine;

// player health
// other scripts listen to these events instead of checking health every frame
public class Health : MonoBehaviour
{
    public int maxHealth = 5;
    public float hitCooldown = 1f; // so one hazard doesn't take all your health at once

    public int Current { get; private set; }

    public event Action<int, int> Damaged; // current, max
    public event Action<int, int> Healed;  // current, max
    public event Action Died;

    float lastHitTime = -100f;

    void Awake()
    {
        Current = maxHealth;
    }

    public void TakeDamage(int amount)
    {
        if (Current <= 0) return;
        if (Time.time < lastHitTime + hitCooldown) return;

        lastHitTime = Time.time;
        Current = Mathf.Max(Current - amount, 0);
        Damaged?.Invoke(Current, maxHealth);

        if (Current == 0)
            Died?.Invoke();
    }

    public void ResetHealth()
    {
        Current = maxHealth;
        lastHitTime = Time.time; // little bit of safe time after respawning
        Healed?.Invoke(Current, maxHealth);
    }
}
