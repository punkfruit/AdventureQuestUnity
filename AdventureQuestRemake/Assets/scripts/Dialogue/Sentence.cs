using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class Sentence : ScriptableObject
{
    public string characterName;
    [TextArea(3, 10)]
    public string text;
    
    public abstract void Execute();
}


/*
[CreateAssetMenu(fileName = "New Quest Sentence", menuName = "Dialogue/Quest Sentence")]
public class QuestSentence : Sentence
{
    public string questID;

    public override void Execute()
    {
        // Code for quest-related logic
    }
}


*/
