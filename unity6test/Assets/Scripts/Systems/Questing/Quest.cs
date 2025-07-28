using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "New Quest", menuName = "Quest System/Quest")]
public class Quest : ScriptableObject
{
    public int questID;
    public string questName; //example: run 10 miles!
    [TextArea(3, 10)]
    public string questDescription;
    public Sprite questIcon;
    
    public QuestElement[] questElements;
    
}

[System.Serializable]
public class QuestElement
{
    public string elementName; //example: Ran first mile! 9 left!
    [TextArea(3, 10)]
    public string elementDescription;
    public bool elementComplete;

    public QuestElement(string elementName, string elementDescription)
    {
        this.elementName = elementName;
        this.elementDescription = elementDescription;
    }
}
