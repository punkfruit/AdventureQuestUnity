using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class Interactable : MonoBehaviour
{
    public string uniqueID;
    public bool state = false; //default state, ie chest being closed

    public bool inRange = false;//weather the player is in range of the object
    public bool savable = true;
    public bool triggerOnEnter = false;
    //public BoxCollider2D boxCollider;
    public Collider2D collider2D;
    public GameObject indicator;

    public UnityEvent OnInteractPerformedEvent;
    public UnityEvent<bool> ApplyStateEvent;
    public PlayerInput playerInput;

    private void Start()
    {
        if(savable)
        {
            state = SaveManager.Instance.GetObjectState(uniqueID);
            ApplyState(state);
        }
        
        if(!triggerOnEnter)
            indicator.SetActive(false);
        
        playerInput = GameManager.instance.playerInput;
        //SaveManager.Instance.SetObjectState(uniqueID, state);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player" && !state)
        {
            if (triggerOnEnter)
            {
                inRange = true;
                playerInput.actions["Interact"].performed += OnInteractPerformed;
                OnInteractPerformedEvent?.Invoke();
            }
            else
            {
                inRange = true;
                indicator.SetActive(true);
                playerInput.actions["Interact"].performed += OnInteractPerformed;
            }
            
        }
    }
    
    
    

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            inRange = false;
            
            playerInput.actions["Interact"].performed -= OnInteractPerformed;
            if(indicator != null)
                indicator.SetActive(false);
        }
    }

    public void ApplyState(bool state)
    {
        // Replace this with logic that reflects the state, like animation or enabling/disabling a collider
        ApplyStateEvent?.Invoke(state);
        Debug.Log($"Applying state to {uniqueID}: {state}");

    }

    public void OnInteractPerformed(InputAction.CallbackContext context)
    {
        OnInteractPerformedEvent?.Invoke();
    }
}
