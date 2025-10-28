using Unity.VisualScripting;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public Interactable baseInteractable;
    public Dialogue dialogue;
    public SpriteRenderer spr;
   
    

    public void TriggerDialogue()
    {
        if(!DialogueManager.instance.dialogueIsPlaying)
        {
            DialogueManager.instance.StartDialogue(dialogue);
            PlayerController.instance.canMove = false;
            if (baseInteractable.indicator != null)
            {
                baseInteractable.indicator.SetActive(false);
            }
                

            
        }
        else
        {
            if (DialogueManager.instance.DisplayNextSentence())
            {
                if (baseInteractable.savable)
                {
                    baseInteractable.state = true;
                    SaveManager.Instance.SetObjectState(baseInteractable.uniqueID, baseInteractable.state);
                    ApplyState(baseInteractable.state);
                }
            }
        }
    }
    
    
    public void ApplyState(bool stat) //listening to the base interactable apply state event. hate cluttering my beautiful and simple dialogue trigger but oh well
    {
        if(stat == true)
        {
            baseInteractable.collider2D.enabled = false;
            if (spr != null)
            {
                spr.enabled = false;
            }
                
        }
        else
        {
            baseInteractable.collider2D.enabled = true;
            if (spr != null)
            {
                spr.enabled = true;
            }
        }
    }

}
