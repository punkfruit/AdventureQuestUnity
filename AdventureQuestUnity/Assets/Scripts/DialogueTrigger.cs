using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public Dialogue dialogue;
    public Collider2D collid;
    public bool sign, playerInRange;
    //public GameObject dialogueBox;
    public Animator dialogueAnim;

    public void TriggerDialogue()
    {
        //FindObjectOfType<DialogueManager>().StartDialogue(dialogue);
        DialogueManager.instance.StartDialogue(dialogue);
        //dialogueBox.SetActive(true);
    }

    private void Update()
    {
        if (playerInRange)
        {
            if (Input.GetButtonDown("Action"))
            {
                if (dialogueAnim.GetBool("IsOpen") == false)
                {
                    TriggerDialogue();
                    
                }
                else
                {
                    //FindObjectOfType<DialogueManager>().DisplayNextSentence();
                    DialogueManager.instance.DisplayNextSentence();
                }
                
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            playerInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            playerInRange = false;
        }
    }
}
