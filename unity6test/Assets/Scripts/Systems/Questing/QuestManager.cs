using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuestManager : MonoBehaviour
{
    public static QuestManager instance;
    
    public List<Quest> quests; //current list of active quests

    
    public QuestList questList; //scriptable object whose only job is to hold a reference to every quest in the game
    

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

    public void AddQuest(Quest questToAdd) //adding quest for the first time! use future LoadQuest for any other time
    {
        quests.Add(questToAdd);
        for (int i = 0; i < questToAdd.questElements.Length; i++)
        {
            questToAdd.questElements[i].elementComplete = false; //resets quest to base state for when adding it for the first time
        }

        UIManager.Instance.ShowNotification("Quest Added", "!");
        SaveQuestProgress();
    }

    public void AddQuestWithInt(int num)
    {
        quests.Add(questList.quests[num]);
    }

    public bool QuestAlreadyAdded(Quest questToCheck)
    {
        for (int i = 0; i < quests.Count; i++)
        {
            if (questToCheck.questID == quests[i].questID)
            {
                return true;
            }
        }
        
        return false;
    }

    public void LoadQuestProgress()
    {
        quests.Clear();

        foreach (var savedQuest in SaveManager.Instance.CurrentData.activeQuests)
        {
            Quest questSO = Array.Find(QuestManager.instance.questList.quests, q => q.questID == savedQuest.questID);
            

            if (questSO != null)
            {
                // Clone a fresh instance so we don't mutate the original ScriptableObject
                Quest clonedQuest = Instantiate(questSO);
                for (int i = 0; i < savedQuest.elementsCompleted.Length; i++)
                {
                    clonedQuest.questElements[i].elementComplete = savedQuest.elementsCompleted[i];
                }

                quests.Add(clonedQuest);
            }
            else
            {
                Debug.LogWarning("Quest ID not found in QuestList: " + savedQuest.questID);
            }
        }
    }

    public void SaveQuestProgress()
    {
        SaveManager.Instance.CurrentData.activeQuests.Clear();

        foreach (var quest in quests)
        {
            var entry = new SaveManager.GameData.QuestProgressEntry
            {
                questID = quest.questID,
                elementsCompleted = new bool[quest.questElements.Length]
            };

            for (int i = 0; i < quest.questElements.Length; i++)
            {
                entry.elementsCompleted[i] = quest.questElements[i].elementComplete;
            }

            SaveManager.Instance.CurrentData.activeQuests.Add(entry);
        }
    }


    public void AdvanceElement(int questID, int elementID)
    {
        for (int i = 0; i < quests.Count; i++)
        {
            if (quests[i].questID == questID)
            {
                quests[i].questElements[elementID].elementComplete = true;
                //UIManager.Instance.ShowNotification("Quest Updated", "!");
                return;
            }
        }
    }
}
