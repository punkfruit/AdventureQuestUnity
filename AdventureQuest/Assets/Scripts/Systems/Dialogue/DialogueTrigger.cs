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
    public GameObject visulCue;


    public PlayerInput playerInput;
    public SpriteRenderer uiconSprite;
    public Sprite[] icons;

    private void Start()
    {
        playerInput = PlayerController.instance.playerInput;
        if (playerInput != null)
        {
            playerInput.actions["Dialogue"].performed += OnDialoguePerformed;
        }

        //GameManager.OnInputDeviceChanged += UpdateControlSprite;
    }

    private void UpdateControlSprite(string deviceType)
    {
        uiconSprite.sprite = deviceType == "Keyboard" ? icons[0] : icons[1];

    }

    private void OnDialoguePerformed(InputAction.CallbackContext context)
    {
        if (!UIManager.instance.inventoryOpen)
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
                PlayerController.instance.moveDirection = Vector2.zero;

            }
            else
            {
                DialogueManager.instance.DisplayNextSentence();


            }


        }

    }
}
