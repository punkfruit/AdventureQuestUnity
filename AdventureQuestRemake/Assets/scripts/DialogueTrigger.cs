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

    

    private void Start()
    {
        //dialogueAnim = 
    }

    private void Update()
    {

        /*
        if (playerInRange)
        {


            if (Input.GetButtonDown("Action"))
            {
                if (dialogueAnim.GetBool("isOpen") == false)
                {
                    TriggerDialogue();



                }
                else
                {
                    DialogueManager.instance.DisplayNextSentence();


                }
            }


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

        */

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


    public void TriggerDialogue(InputAction.CallbackContext context)
    {
        if (context.performed)
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
}
