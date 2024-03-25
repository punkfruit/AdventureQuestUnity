using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu]
public class QuestList : ScriptableObject
{
    public Quest[] Quest_List; //quest ID should match its position in the array
}
