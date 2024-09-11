using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager instance;
    private Sentence currentSentence;

    public TextMeshProUGUI nameText, dialogueText;
    //private Queue<string> sentences;
    private Queue<Sentence> sentences;
    public Animator dialogueAnim;

    public Image faceIcon;
    public bool dialogueIsPlaying;

    public bool sentenceIsTyping;

    public float typeSpeed = 0.03f;
    public string sentence;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
    }

    private void Start()
    {
        dialogueIsPlaying = false;
        sentences = new Queue<Sentence>();
    }



    public void StartDialogue(Dialogue dialogue)
    {
        dialogueIsPlaying = true;
        sentenceIsTyping = false;
        dialogueAnim.SetBool("isOpen", true);
        PlayerController.instance.canMove = false;
        sentences.Clear();

        foreach (Sentence sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }
        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        if (sentences.Count == 0 && !sentenceIsTyping)
        {
            EndDialogue();
            return;
        }

        StopAllCoroutines();
        if (sentenceIsTyping)
        {
            ShowSentence(currentSentence.text);
        }
        else
        {
            currentSentence = sentences.Dequeue();
            nameText.text = currentSentence.characterName;
            faceIcon.sprite = currentSentence.characterIcon;
            StartCoroutine(TypeSentence(currentSentence.text));
        }
    }




    IEnumerator TypeSentence(string sentenceText)
    {
        sentenceIsTyping = true;
        dialogueText.text = "";
        foreach (char letter in sentenceText.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(typeSpeed);
        }
        sentenceIsTyping = false;
    }


    public void ShowSentence(string sentence)
    {
        dialogueText.text = sentence;
        sentenceIsTyping = false;
    }

    public void EndDialogue()
    {
        dialogueAnim.SetBool("isOpen", false);
        PlayerController.instance.canMove = true;
        dialogueIsPlaying = false;
        Debug.Log("end log");
    }
}
