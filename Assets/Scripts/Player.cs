using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public float speed = 5f;
    private Vector2 moveInput;
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        moveInput = Vector2.zero;


        if (Keyboard.current.leftArrowKey.isPressed) moveInput.x = -1;
        if (Keyboard.current.rightArrowKey.isPressed) moveInput.x = 1;
        if (Keyboard.current.upArrowKey.isPressed) moveInput.y = 1;
        if (Keyboard.current.downArrowKey.isPressed) moveInput.y = -1;
    }

    private void FixedUpdate() {
        rb.linearVelocity = moveInput * speed;
    }
}
