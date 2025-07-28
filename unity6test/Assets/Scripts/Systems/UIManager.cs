using System.Collections;
using System.Collections.Generic;
//using UnityEditor.Tilemaps;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class UIManager : MonoBehaviour
{

    public static UIManager Instance;

    public PlayerInput playerInput;

    [Header("Inventory")]
    public GameObject inventoryPanel;
    public Button initialInventoryButton;
    public Inventory _inventoryPanel;
    public bool inventoryOpen = false;


    [Header("Pause Menu")]
    public GameObject pauseMenu;
    public bool isPaused = false;
    public bool optionsPanelActive = false;
    public GameObject optionsPanel, buttonsHolder;
    public Button initialPauseButton;
    public Slider initialVolumeSlider;

    [Header("Notification Box")]
    public TextMeshProUGUI exclamationText;
    public TextMeshProUGUI notifText;
    public Animator notifAnim;
    
    [Header("Questing")]
    public GameObject questingPanel;
    public bool questingOpen = false;
    public TextMeshProUGUI elementName;
    public TextMeshProUGUI elementDescription, questDescription;
    public Image questIcon;
    public Sprite defaultIcon;
    public GameObject _currentQuestSelection;
    public GameObject questButtonPrefab;
    public Transform questGrid;
    public Scrollbar scrollbar;
    public ScrollRectAutoScroll scroller;
    public WaitForSeconds seconds;
    


    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    void Start()
    {
        //playerInput = PlayerController.instance.playerInput;
        if (playerInput != null)
        {
            playerInput.actions["OpenInventory"].performed += OnInventoryOpenPerformed;
            playerInput.actions["CloseInventory"].performed += OnInventoryClosePerformed;
            playerInput.actions["Escape"].performed += OnEscapePerformed;
            playerInput.actions["EscapeUI"].performed += OnEscapeUIPerformed;
            playerInput.actions["OpenQuest"].performed += OnQuestOpenPerformed;
            playerInput.actions["CloseQuest"].performed += OnQuestClosePerformed;

            playerInput.actions["Cancel"].performed += OnCancelPerformed;
        }

        CloseInventory(); // Ensure inventory is initially closed
        optionsPanel.SetActive(false);
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false; // Ensure the pause menu is initally closed
        
        seconds = new WaitForSeconds(0.05f);
    }

    private void OnEscapePerformed(InputAction.CallbackContext context)
    {
        if(!DialogueManager.instance.dialogueIsPlaying && !isPaused && !questingOpen)
        {
           PauseGame();
        }
    }

    private void OnEscapeUIPerformed(InputAction.CallbackContext context)
    {
        if(isPaused)
        {
            if(optionsPanelActive)
            {
                HideOptions();
            }
            else
            {
                UnPauseGame();
            }
            
        }
    }

    private void OnCancelPerformed(InputAction.CallbackContext context)
    {
        if (isPaused)
        {
            if (optionsPanelActive)
            {
                HideOptions();
            }
            else
            {
                UnPauseGame();
            }
        }

        if (inventoryOpen)
        {
            CloseInventory();
        }

        if (questingOpen)
        {
            HideQuestMenu();
        }
    }

    private void OnQuestOpenPerformed(InputAction.CallbackContext context)
    {
        if (!questingOpen)
        {
            ShowQuestMenu();
        }
    }

    private void OnQuestClosePerformed(InputAction.CallbackContext context)
    {
        if (questingOpen)
        {
            HideQuestMenu();
        }
    }

    private void OnInventoryOpenPerformed(InputAction.CallbackContext context)
    {
        if (!inventoryOpen)
        {
            OpenInventory();
        }
    }

    private void OnInventoryClosePerformed(InputAction.CallbackContext context)
    {
        if (inventoryOpen)
        {
            CloseInventory();
        }
    }

    public void CloseInventory()
    {
        inventoryPanel.SetActive(false);
        _inventoryPanel.DeselectSlot();
        playerInput.SwitchCurrentActionMap("Player");

        inventoryOpen = false;
        //Debug.Log("Inventory closed");
    }

    public void OpenInventory()
    {
        if (!DialogueManager.instance.dialogueIsPlaying && !isPaused && !questingOpen)
        {
            inventoryPanel.SetActive(true);
            playerInput.SwitchCurrentActionMap("UI");
            _inventoryPanel.DeselectSlot();
            //initialInventoryButton.Select();
            _inventoryPanel.SelectSlot(0);

            inventoryOpen = true;

            //Debug.Log("Inventory opened");
        }
    }

    public void PauseGame()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
        CloseInventory();
        playerInput.SwitchCurrentActionMap("UI");

        initialPauseButton.Select();
    }

    public void UnPauseGame()
    {
        optionsPanel.SetActive(false);
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
        playerInput.SwitchCurrentActionMap("Player");
    }

    public void ShowOptions()
    {
        buttonsHolder.SetActive(false);
        optionsPanel.SetActive(true);
        initialVolumeSlider.Select();
        optionsPanelActive = true;
    }

    public void HideOptions()
    {
        buttonsHolder.SetActive(true);
        optionsPanel.SetActive(false);
        initialPauseButton.Select();
        optionsPanelActive = false;
    }

    public void ShowNotification(string notif, string exclamation)
    {
        notifAnim.ResetTrigger("ActivateNotif");
        notifText.text = notif;
        exclamationText.text = exclamation;
        notifAnim.SetTrigger("ActivateNotif");
    }

    public void ShowQuestMenu()
    {
        if (!isPaused && !inventoryOpen && !DialogueManager.instance.dialogueIsPlaying)
        {
            questingPanel.SetActive(true);
            playerInput.SwitchCurrentActionMap("UI");
            questingOpen = true;
            RefreshQuestUI();
            StartCoroutine(TurnOnScroller());
        }
    }

    public void HideQuestMenu()
    {
        questingPanel.SetActive(false);
        playerInput.SwitchCurrentActionMap("Player");
        scroller.enabled = false;
        questingOpen = false;
    }

    public void RefreshQuestUI()
    {
        foreach (Transform child in questGrid)
        {
            Destroy(child.gameObject);
        }
        
        elementName.text = string.Empty;
        elementDescription.text = string.Empty;
        questDescription.text = string.Empty;
        questIcon.sprite = null;
        
        List<GameObject> questButtons = new List<GameObject>();

        foreach (Quest quest in QuestManager.instance.quests)
        {
            GameObject buttonObj = Instantiate(questButtonPrefab, questGrid);
            
            TextMeshProUGUI buttonText = buttonObj.GetComponentInChildren<TextMeshProUGUI>();
            buttonText.text = quest.questName;
            
            Image[] img = buttonObj.GetComponentsInChildren<Image>();
            if (quest.questIcon != null)
            {
                img[1].sprite = quest.questIcon;
            }
            else
            {
                img[1].sprite = defaultIcon;
            }
            
            Button button = buttonObj.GetComponent<Button>();
            button.onClick.AddListener(() => OnQuestSelected(quest));
            
            questButtons.Add(buttonObj);
        }

        if (questButtons.Count > 0)
        {
            EventSystem.current.SetSelectedGameObject(questButtons[0]);
        }
        else
        {
            scrollbar.Select();
        }
        
        
    }

    public void OnQuestSelected(Quest quest)
    {
        questDescription.text = quest.questDescription;
        if (quest.questIcon != null)
        {
            questIcon.sprite = quest.questIcon;
        }
        else
        {
            questIcon.sprite = defaultIcon;
        }

        foreach (QuestElement element in quest.questElements)
        {
            if (!element.elementComplete)
            {
                elementName.text = element.elementName;
                elementDescription.text = element.elementDescription;
                break;
            }
        }
    }

    public IEnumerator TurnOnScroller()
    {
        yield return seconds;
        
        scroller.enabled = true;
        
        yield return seconds;
        
        scroller.ScrollToSelected(false);
    }
}
