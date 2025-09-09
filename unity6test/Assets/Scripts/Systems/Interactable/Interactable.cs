using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class Interactable : MonoBehaviour
{
    public string uniqueID;
    public bool state = false; //default state, ie chest being closed

    public bool inRange = false;//weather the player is in range of the object
    public bool savable = true;
    public GameObject indicator;

    public UnityEvent OnInteractPerformedEvent;
    public UnityEvent ApplyStateEvent;
    public PlayerInput playerInput;

    private void Start()
    {
        if(savable)
        {
            state = SaveManager.Instance.GetObjectState(uniqueID);
            ApplyState(state);
        }
        
        indicator.SetActive(false);
        playerInput = GameManager.instance.playerInput;
        //SaveManager.Instance.SetObjectState(uniqueID, state);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player" && !state)
        {
            inRange = true;
            indicator.SetActive(true);
            playerInput.actions["Interact"].performed += OnInteractPerformed;
        }
    }
    
    
    

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            inRange = false;
            indicator.SetActive(false);
            playerInput.actions["Interact"].performed -= OnInteractPerformed;
        }
    }

    public void ApplyState(bool state)
    {
        // Replace this with logic that reflects the state, like animation or enabling/disabling a collider
        ApplyStateEvent?.Invoke();
        Debug.Log($"Applying state to {uniqueID}: {state}");

    }

    public void OnInteractPerformed(InputAction.CallbackContext context)
    {
        OnInteractPerformedEvent?.Invoke();
    }
}
