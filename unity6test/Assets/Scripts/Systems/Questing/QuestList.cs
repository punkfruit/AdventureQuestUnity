using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "QuestList", menuName = "ScriptableObjects/QuestList")]
public class QuestList : ScriptableObject
{
    public Quest[] quests; //quest ID should match its position in the array
}
