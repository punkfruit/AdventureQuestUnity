using MoreMountains.InventoryEngine;
using MoreMountains.Tools;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;

public class QuestManager : MonoBehaviour
{
    public static QuestManager instance;

    public List<Quest> quests;


    public GameObject _currentSelection;
    public GameObject QuestButtonPrefab;
    public Transform QuestGrid;
    public Scrollbar Scrollbar;


    [Header("Quest Details")]
    public TextMeshProUGUI ElementName;
    public TextMeshProUGUI ElementDescription, QuestDescription;
    public Image icon;
    public Sprite defaultIcon;

    public QuestList QuestHolder;
    public bool QuestMenuOpen = false;

    public bool canOpenQuestMenu = true;
    public int test;
    public int[] testArray;

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
        if (!PlayerPrefs.HasKey("QuestList"))
        {
            PlayerPrefs.SetString("QuestList", "");
        }
        else
        {
            LoadQuest();
        }
    }

    public void AddQuest(Quest questToAdd) //adding a quest for the first time!! use future LoadQuest for any other time
    {
        quests.Add(questToAdd);
        for(int i = 0; i < questToAdd.questElements.Length; i++)
        {
            questToAdd.questElements[i].elementCompleted = false; //resets quest to base state for when adding it for the first time.
        }

        SaveQuestList();
    }

    public void AddQuestWithInt(int num)
    {
        quests.Add(QuestHolder.Quest_List[num]);
    }

    public bool QuestAlreadyAdded(Quest questToCheck)
    {
        for(int i = 0; i < quests.Count; i++)
        {
            if(questToCheck.QuestID == quests[i].QuestID)
            {
                return true;
            }
        }

        return false;
    }

    public void LoadQuest()
    {
        string testString = PlayerPrefs.GetString("QuestList");
        // Split the string into an array of strings using the comma as a separator
        string[] questIdStrings = testString.Split(',');
        quests.Clear();
        // Iterate over each string in the array
        foreach (string questIdString in questIdStrings)
        {
            // Convert the string to an integer
            int questId = Int32.Parse(questIdString);

            // Call your method with the integer
            AddQuestWithInt(questId);
        }
    }

    public void SaveQuestList()
    {

        string testString = "";
        foreach(Quest quest in quests)
        {
            testString = (testString + quest.QuestID + ",");
        }

        PlayerPrefs.SetString("QuestList", testString);
    }

    private void Update()
    {
        //CheckCurrentlySelectedSlot();
    }

    public void CheckCurrentlySelectedSlot()
    {
        _currentSelection = EventSystem.current.currentSelectedGameObject;
        if (_currentSelection == null)
        {
            return;
        }


       // StartCoroutine(ScrollToSelected());
    }


    public void RefreshQuestUI()
    {
        QuestGrid.MMDestroyAllChildren();
        ElementName.text = string.Empty;
        ElementDescription.text = string.Empty;
        QuestDescription.text = string.Empty;
        icon.sprite = null;

        List<GameObject> questButtons = new List<GameObject>();

        foreach (Quest quest in quests)
        {
            GameObject buttonObj = Instantiate(QuestButtonPrefab, QuestGrid);

            // Set button text
            TextMeshProUGUI buttonText = buttonObj.GetComponentInChildren<TextMeshProUGUI>();
            buttonText.text = quest.questName;

            //setup button icon
            Image[] img = buttonObj.GetComponentsInChildren<Image>();
            if(quest.questIcon != null)
            {
                img[1].sprite = quest.questIcon;
            }
            else
            {
                img[1].sprite = defaultIcon;
            }

            //Debug.Log(img.Length);
                

            // Add OnClick listener
            Button button = buttonObj.GetComponent<Button>();
            button.onClick.AddListener(() => OnQuestSelected(quest));

            questButtons.Add(buttonObj);
        }

        // Setup navigation for buttons and scrollbar
        for (int i = 0; i < questButtons.Count; i++)
        {
            Navigation navigation = new Navigation();
            navigation.mode = Navigation.Mode.Explicit;
            Button button = questButtons[i].GetComponent<Button>();

            navigation.selectOnUp = (i > 0) ? questButtons[i - 1].GetComponent<Button>() : null;
            navigation.selectOnDown = (i < questButtons.Count - 1) ? questButtons[i + 1].GetComponent<Button>() : null;
            navigation.selectOnRight = Scrollbar.GetComponent<Selectable>();

            button.navigation = navigation;
        }

        // Set up scrollbar navigation
        Navigation scrollbarNavigation = new Navigation();
        scrollbarNavigation.mode = Navigation.Mode.Explicit;
        scrollbarNavigation.selectOnLeft = questButtons.Count > 0 ? questButtons[0].GetComponent<Button>() : null; // Assumes first quest button as the target
        Scrollbar.navigation = scrollbarNavigation;


        // Set the first button as the selected object
        if (questButtons.Count > 0)
        {
            EventSystem.current.SetSelectedGameObject(questButtons[0]);
        }




    }

    





    public void OnQuestSelected(Quest quest)
    {
        
        QuestDescription.text = quest.questDescription;
        if(quest.questIcon != null)
        {
            icon.sprite = quest.questIcon;
        }
        else
        {
            icon.sprite = defaultIcon;
        }

        foreach (QuestElement element in quest.questElements)
        {
            if (!element.elementCompleted)
            {
                ElementName.text = element.elementName;
                ElementDescription.text = element.elementDescription;
                break;
            }
        }

        
    }

    private IEnumerator ResetScrollPosition()
    {
        // Wait for end of frame to ensure all UI elements are updated
        yield return new WaitForEndOfFrame();

        // Reset the scrollbar value to the top (1 for vertical scrollbars)
        if (Scrollbar != null)
        {
            Scrollbar.value = 1; // 1 for top, 0 for bottom
        }
        else
        {
            // If no scrollbar, reset content panel's position directly
            RectTransform contentPanel = QuestGrid.GetComponent<RectTransform>();
            contentPanel.anchoredPosition = new Vector2(contentPanel.anchoredPosition.x, 0);
        }
    }


    public void AdvanceElement(int questID, int ElementID)
    {
        for (int i = 0; i < quests.Count; i++)
        {
            if(quests[i].QuestID == questID)
            {
                quests[i].questElements[ElementID].elementCompleted = true;
                return;
            }
        }
    }


}
