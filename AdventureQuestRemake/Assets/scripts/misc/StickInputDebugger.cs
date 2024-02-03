using UnityEngine;
using UnityEngine.InputSystem;

public class StickInputDebugger : MonoBehaviour
{
    public InputActionAsset inputActionAsset; // Assign in inspector
    private InputAction movementAction;

    private void Awake()
    {
        // Assumes you have a "Movement" action in your Input Actions
        movementAction = inputActionAsset.FindActionMap("UI").FindAction("Navigate");

        // Subscribe to the performed event to handle input
        movementAction.performed += OnMovementPerformed;
    }

    private void OnMovementPerformed(InputAction.CallbackContext context)
    {
        // Get the Vector2 value from the stick
        Vector2 stickInput = context.ReadValue<Vector2>();

        // Determine the direction based on the stick input
        string direction = Vector2ToDirection(stickInput);

        // Log the direction
        Debug.Log("Direction: " + direction);
    }

    private string Vector2ToDirection(Vector2 input)
    {
        if (input.magnitude < 0.1f) // Adjust deadzone sensitivity as needed
            return "Center";

        if (Mathf.Abs(input.x) > Mathf.Abs(input.y))
        {
            // Horizontal movement is stronger
            return input.x > 0 ? "Right" : "Left";
        }
        else
        {
            // Vertical movement is stronger
            return input.y > 0 ? "Up" : "Down";
        }
    }

    private void OnEnable()
    {
        movementAction.Enable();
    }

    private void OnDisable()
    {
        movementAction.Disable();
    }
}
