using UnityEngine;
using UnityEngine.UI;

// listener 1 - the health bar
// the lag bar is the lighter one behind the fill. after a hit it waits 0.4s
// and then slides down at 0.8 per second so you can see how much you lost
public class HealthBar : MonoBehaviour
{
    public Health health;
    public Image fill;
    public Image lag;
    public float lagDelay = 0.4f;
    public float lagSpeed = 0.8f;

    float lagTimer;

    void Awake()
    {
        // the HUD is a prefab so it finds the player's health itself
        if (health == null) health = FindFirstObjectByType<Health>();
    }

    void OnEnable()
    {
        if (health == null) return;
        health.Damaged += OnHealthChanged;
        health.Healed += OnHealthChanged;
    }

    void OnDisable()
    {
        if (health == null) return;
        health.Damaged -= OnHealthChanged;
        health.Healed -= OnHealthChanged;
    }

    void Start()
    {
        if (health == null) return;
        Set(health.Current, health.maxHealth);
        lag.fillAmount = fill.fillAmount;
    }

    void OnHealthChanged(int current, int max)
    {
        Set(current, max);
    }

    public void Set(int current, int max)
    {
        float amount = max > 0 ? (float)current / max : 0f;
        fill.fillAmount = amount;

        if (lag.fillAmount < amount)
            lag.fillAmount = amount; // got health back, no need to lag
        else
            lagTimer = lagDelay;     // took damage, wait a bit first
    }

    void Update()
    {
        if (lagTimer > 0f)
        {
            lagTimer -= Time.deltaTime;
            return;
        }

        if (lag.fillAmount > fill.fillAmount)
            lag.fillAmount = Mathf.MoveTowards(lag.fillAmount, fill.fillAmount, lagSpeed * Time.deltaTime);
    }
}
