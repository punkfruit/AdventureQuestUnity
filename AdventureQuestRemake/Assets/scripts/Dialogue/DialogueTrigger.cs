using MoreMountains.InventoryEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DialogueTrigger : MonoBehaviour
{
    public Dialogue dialogue;
    public Collider2D collid;
    public bool sign, playerInRange, canTriggerDialogue = true;
    //public Animator dialogueAnim;

    public PlayerInput playerInput;
    private InventoryInputManager inventoryInputManager;

    public GameObject visulCue;

    public WaitForSeconds time = new WaitForSeconds(0.5f);



    private void Start()
    {
        playerInput = PlayerController.instance.playerInput;
        
        // Find the InventoryInputManager instance
        inventoryInputManager = FindObjectOfType<InventoryInputManager>();
    }



    private void OnDialoguePerformed(InputAction.CallbackContext context)
    {
        // Check if the inventory is not open before triggering dialogue
        if (inventoryInputManager != null && !inventoryInputManager.InventoryIsOpen)
        {
            if (!QuestManager.instance.QuestMenuOpen)
            {
                TriggerDialogue();
            }
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
            playerInput.actions["Dialogue"].performed += OnDialoguePerformed;


        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            playerInRange = false;
            playerInput.actions["Dialogue"].performed -= OnDialoguePerformed;

            if (visulCue != null)
                visulCue.SetActive(false);
        }
    }


    public void TriggerDialogue()
    {
        Debug.Log("dialogue triggered");
        if (playerInRange)
        {


            if (!DialogueManager.instance.IsDialoguePlaying && canTriggerDialogue)
            {

                DialogueManager.instance.StartDialogue(dialogue);
                DialogueManager.instance.trig = this;

                canTriggerDialogue = false;

            }



        }



    }

    public IEnumerator EndDialogueFixCO()
    {
        yield return time;
        canTriggerDialogue = true;
    }

    public void EndDialogueFix()
    {
        StartCoroutine(EndDialogueFixCO()); //the dialogue manager cant start the coroutine here so i need 2 methods to start it ugh
    }
}