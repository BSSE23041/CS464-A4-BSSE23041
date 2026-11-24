using UnityEngine;

// camera follows the player from above and a bit behind
// listener 3 - it also shakes a little when the player gets hit
public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public Health health;
    public Vector3 offset = new Vector3(0f, 14f, -7f);
    public float smooth = 8f;
    public float shakeTime = 0.25f;
    public float shakeAmount = 0.3f;

    float shakeTimer;

    void OnEnable() => health.Damaged += OnHit;
    void OnDisable() => health.Damaged -= OnHit;

    void OnHit(int current, int max)
    {
        shakeTimer = shakeTime;
    }

    void LateUpdate()
    {
        if (target == null) return;

        Vector3 wanted = target.position + offset;
        transform.position = Vector3.Lerp(transform.position, wanted, smooth * Time.deltaTime);
        transform.LookAt(target.position);

        if (shakeTimer > 0f)
        {
            shakeTimer -= Time.deltaTime;
            transform.position += Random.insideUnitSphere * shakeAmount;
        }
    }
}
