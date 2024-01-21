using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu]
public class Quest : ScriptableObject
{
    public int QuestID;
    public string questName;
    [TextArea(3, 10)]
    public string questDescription;
    public Sprite questIcon;

    public QuestElement[] questElements;
}


[System.Serializable]
public class QuestElement
{
    public string elementName;
    [TextArea(3, 10)]
    public string elementDescription;

    public bool elementCompleted;

    public QuestElement(string elementName, string elementDescription)
    {
        this.elementName = elementName;
        this.elementDescription = elementDescription;
    }
}
