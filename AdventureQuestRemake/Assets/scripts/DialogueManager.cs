using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager instance;

    public TextMeshProUGUI nameText, dialogueText;
    private Queue<string> sentences;
    public Animator dialogueAnim;

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
        sentences = new Queue<string>();
    }

    public void StartDialogue(Dialogue dialogue)
    {
        dialogueIsPlaying = true;
        sentenceIsTyping = false;
        dialogueAnim.SetBool("isOpen", true);
        PlayerController.instance.canMove = false;
        nameText.text = dialogue.name;
        sentences.Clear();

        foreach (string sentence in dialogue.sentences)
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

            ShowSentence(sentence);
        }
        else
        {
            sentence = sentences.Dequeue();
            StartCoroutine(TypeSentence(sentence));
        }


    }

    IEnumerator TypeSentence(string sentence)
    {
        sentenceIsTyping = true;
        dialogueText.text = "";
        foreach (char letter in sentence.ToCharArray())
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
