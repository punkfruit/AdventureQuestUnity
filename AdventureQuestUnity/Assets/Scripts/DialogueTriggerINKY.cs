using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTriggerINKY : MonoBehaviour
{
    public Animator dialogueAnim;
    [Header("Visual Cue")]
    [SerializeField] private GameObject visualCue;

    [Header("Ink JSON")]
    [SerializeField] private TextAsset inkJSON;
    //public string name;
    private bool playerInRange;
    public bool npc = false;
    public SpriteRenderer spr;


    private void Awake()
    {
        playerInRange = false;
        visualCue.SetActive(false);
    }

    private void Update()
    {
        if (playerInRange && !DialogueManagerInky.GetInstance().dialogueIsPlaying)
        {
            visualCue.SetActive(true);
            if (Input.GetButtonDown("Action"))
            {
                if (dialogueAnim.GetBool("IsOpen") == false)
                {
                    DialogueManagerInky.GetInstance().EnterDialogueMode(inkJSON, name);
                    if (npc)
                    {
                        if(PlayerController.instance.transform.position.x < transform.position.x)
                        {
                            spr.flipX = true;
                        }
                        else
                        {
                            spr.flipX = false;
                        }
                    }
                }
                    
            }
        }
        else
        {
            visualCue.SetActive(false);
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
        }
    }
}
