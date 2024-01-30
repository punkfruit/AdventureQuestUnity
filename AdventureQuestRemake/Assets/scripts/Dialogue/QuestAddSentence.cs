using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Quest Addition Sentence", menuName = "Dialogue/Quest Add Sentence")]
public class QuestAddSentence : Sentence
{
    public Quest questToAdd;
    public string notifText = "Quest Added!";
    public override void Execute()
    {

        if (QuestManager.instance.QuestAlreadyAdded(questToAdd))
        {
            return;
        }
        QuestManager.instance.AddQuest(questToAdd);
        GameManager.instance.notificationManager.ShowNotification(notifText);
    }
}
