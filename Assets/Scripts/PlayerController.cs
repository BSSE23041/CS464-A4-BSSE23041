using UnityEngine;
using UnityEngine.InputSystem;

// moves the player with WASD or the arrow keys
[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    public float speed = 6f;

    Rigidbody rb;
    Vector3 moveInput;
    Vector3 startPosition;
    bool inputEnabled = true;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        startPosition = transform.position;
    }

    void Update()
    {
        // Update still runs when timeScale is 0 so I have to check this myself
        if (PauseMenu.IsPaused) return;

        moveInput = Vector3.zero;
        if (!inputEnabled) return;

        Keyboard keyboard = Keyboard.current;
        if (keyboard == null) return;

        float x = 0f;
        float z = 0f;
        if (keyboard.aKey.isPressed || keyboard.leftArrowKey.isPressed) x -= 1f;
        if (keyboard.dKey.isPressed || keyboard.rightArrowKey.isPressed) x += 1f;
        if (keyboard.sKey.isPressed || keyboard.downArrowKey.isPressed) z -= 1f;
        if (keyboard.wKey.isPressed || keyboard.upArrowKey.isPressed) z += 1f;

        moveInput = new Vector3(x, 0f, z).normalized;
    }

    void FixedUpdate()
    {
        Vector3 velocity = moveInput * speed;
        velocity.y = rb.linearVelocity.y; // keep gravity
        rb.linearVelocity = velocity;
    }

    // turned off on the win and game over screens
    public void SetInputEnabled(bool enabled)
    {
        inputEnabled = enabled;
        if (!enabled) moveInput = Vector3.zero;
    }

    public void Respawn()
    {
        rb.linearVelocity = Vector3.zero;
        rb.position = startPosition;
        transform.position = startPosition;
    }
}
