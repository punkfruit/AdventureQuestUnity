using UnityEngine;

public class CharacterController : MonoBehaviour
{
    public float moveSpeed = 5f; // Movement speed of the character
    public Rigidbody2D rb; // Reference to the character's Rigidbody2D

    Vector2 movement; // Stores the direction of movement

    void Update()
    {
        // Input from the player for movement (WASD or Arrow keys)
        movement.x = Input.GetAxisRaw("Horizontal"); // A/D or Left/Right arrow
        movement.y = Input.GetAxisRaw("Vertical");   // W/S or Up/Down arrow
    }

    void FixedUpdate()
    {
        // Applying movement to the Rigidbody2D with moveSpeed
        rb.MovePosition(rb.position + movement.normalized * moveSpeed * Time.fixedDeltaTime);
    }
}
