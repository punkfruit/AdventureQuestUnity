using UnityEngine;

[CreateAssetMenu(fileName = "New Quest Addition Sentence", menuName = "Dialogue/Quest Add Sentence")]
public class QuestAddSentence : Sentence
{
    public Quest questToAdd;
    public override void Execute()
    {
        Debug.Log("sentence executed code");
        if (QuestManager.instance.QuestAlreadyAdded(questToAdd))
        {
            return;
        }
        else
        {
            QuestManager.instance.AddQuest(questToAdd);
        }


    }
}
