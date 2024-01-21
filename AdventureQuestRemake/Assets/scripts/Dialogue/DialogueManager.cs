using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Unity.VisualScripting;
using MoreMountains.InventoryEngine;
using MoreMountains.Tools;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager instance;
    private Sentence currentSentence;

    public TextMeshProUGUI nameText, dialogueText;
    //private Queue<string> sentences;
    private Queue<Sentence> sentences;
    public Animator dialogueAnim;

    //public Image faceIcon;
    public bool dialogueIsPlaying;

    public bool sentenceIsTyping;

    public float typeSpeed = 0.03f;
    public string sentence;

    public PlayerInput playerInput;


    private InventoryInputManager inventoryInputManager;
    public DialogueTrigger trig;

    public GameObject choiceButtonPrefab; // Reference to your button prefab
    public Transform choiceButtonContainer; // The UI container where buttons will be instantiated
    public bool choicesDisplayed = false;

    public InputAction navigateChoices;
    public InputAction selectChoice;

    private WaitForSeconds seconds;
   

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
        navigateChoices.Enable();
        dialogueIsPlaying = false;
        sentences = new Queue<Sentence>();
        inventoryInputManager = FindObjectOfType<InventoryInputManager>();
        playerInput = FindObjectOfType<PlayerInput>();
        if (playerInput != null)
        {
            playerInput.actions["Dialogue"].performed += OnDialoguePerformed;
        }

        seconds = new WaitForSeconds(0.5f);
    }



    public void StartDialogue(Dialogue dialogue)
    {
        dialogueIsPlaying = true;
        ClearChoices();
        inventoryInputManager.canOpenInventory = false;
        QuestManager.instance.canOpenQuestMenu = false;
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
        if (sentences.Count == 0 && !sentenceIsTyping && currentSentence && !choicesDisplayed)
        {
            EndDialogue();
            return;
        }

        if (sentences.Count == 0 && !sentenceIsTyping && currentSentence && choicesDisplayed)
        {
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
            currentSentence.Execute();

            if (currentSentence is SentenceWithChoices sentenceWithChoices)
            {
                // Display the sentence text
                StartCoroutine(TypeSentence(currentSentence.text));
                // Then display the choices
                StartCoroutine(DisplayChoices(sentenceWithChoices.choices));
            }
            else
            {
                StartCoroutine(TypeSentence(currentSentence.text));
            }
        }
    }


    public IEnumerator DisplayChoices(DialogueChoice[] choices)
    {
        yield return seconds; // Delay to prevent immediate selection

        List<GameObject> choiceButtons = new List<GameObject>();
        choicesDisplayed = true;

        // Create buttons for each choice
        foreach (DialogueChoice choice in choices)
        {
            GameObject buttonObj = Instantiate(choiceButtonPrefab, choiceButtonContainer);

            // Set button text
            TextMeshProUGUI buttonText = buttonObj.GetComponentInChildren<TextMeshProUGUI>();
            buttonText.text = choice.choiceText;

            // Add OnClick listener
            Button button = buttonObj.GetComponent<Button>();
            button.onClick.AddListener(() => OnChoiceSelected(choice));

            choiceButtons.Add(buttonObj);
        }

        // Setup navigation for buttons
        for (int i = 0; i < choiceButtons.Count; i++)
        {
            Navigation navigation = new Navigation();
            navigation.mode = Navigation.Mode.Explicit;
            Button button = choiceButtons[i].GetComponent<Button>();

            // Set left and right navigation
            navigation.selectOnLeft = (i > 0) ? choiceButtons[i - 1].GetComponent<Button>() : null;
            navigation.selectOnRight = (i < choiceButtons.Count - 1) ? choiceButtons[i + 1].GetComponent<Button>() : null;

            button.navigation = navigation;
        }

        // Set the first button as the selected object
        if (choiceButtons.Count > 0)
        {
            EventSystem.current.SetSelectedGameObject(choiceButtons[0]);
        }
    }




    public void OnChoiceSelected(DialogueChoice choice)
    {
        // Clear existing buttons
        foreach (Transform child in choiceButtonContainer)
        {
            Destroy(child.gameObject);
        }
        choicesDisplayed = false;
        StartDialogue(choice.nextDialogue);
    }


    public void ClearChoices()
    {
        foreach (Transform child in choiceButtonContainer)
        {
            Destroy(child.gameObject);
        }
        choicesDisplayed = false;
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
        inventoryInputManager.canOpenInventory = true;
        QuestManager.instance.canOpenQuestMenu = true;
        Debug.Log("end log");

        if (trig != null)
            trig.EndDialogueFix();
    }

    public bool IsDialoguePlaying
    {
        get { return dialogueIsPlaying; } // Assuming dialogueIsPlaying is the field that tracks dialogue status
    }


    private void OnDialoguePerformed(InputAction.CallbackContext context)
    {
        if(dialogueIsPlaying)
            DisplayNextSentence();
    }
}
