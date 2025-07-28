using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public Interactable baseInteractable;
    public Dialogue dialogue;

    public void TriggerDialogue()
    {
        if(!DialogueManager.instance.dialogueIsPlaying)
        {
            DialogueManager.instance.StartDialogue(dialogue);
            PlayerController.instance.canMove = false;
            baseInteractable.indicator.SetActive(false);
        }
        else
        {
            DialogueManager.instance.DisplayNextSentence();
        }
    }

}
