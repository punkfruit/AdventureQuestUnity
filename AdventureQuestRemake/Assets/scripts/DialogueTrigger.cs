using MoreMountains.InventoryEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DialogueTrigger : MonoBehaviour
{
    public Dialogue dialogue;
    public Collider2D collid;
    public bool sign, playerInRange;
    public Animator dialogueAnim;

    public PlayerInput playerInput;
    private InventoryInputManager inventoryInputManager;

    public GameObject visulCue;

    

    private void Start()
    {
        playerInput = PlayerController.instance.playerInput;
        if (playerInput != null)
        {
            playerInput.actions["Dialogue"].performed += OnDialoguePerformed;
        }
        // Find the InventoryInputManager instance
        inventoryInputManager = FindObjectOfType<InventoryInputManager>();
    }

    

    private void OnDialoguePerformed(InputAction.CallbackContext context)
    {
        // Check if the inventory is not open before triggering dialogue
        if (inventoryInputManager != null && !inventoryInputManager.InventoryIsOpen)
        {
            TriggerDialogue();
        }
    }


    private void Update()
    {


        if (playerInRange)
        {

            if (DialogueManager.instance.dialogueIsPlaying)
            {
                if (visulCue != null)
                    visulCue.SetActive(false);
            }
            else
            {
                if (visulCue != null)
                    visulCue.SetActive(true);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            playerInRange = true;


        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            playerInRange = false;

            if (visulCue != null)
                visulCue.SetActive(false);
        }
    }


    public void TriggerDialogue()
    {
        
        if (playerInRange)
        {

                
            if (dialogueAnim.GetBool("isOpen") == false)
            {

                DialogueManager.instance.StartDialogue(dialogue);


            }
            else
            {
                DialogueManager.instance.DisplayNextSentence();


            }



        }
        


    }
}
