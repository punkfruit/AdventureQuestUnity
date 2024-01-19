using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Event Sentence", menuName = "Dialogue/Event Sentence")]
public class EventSentence : Sentence
{
    public string eventName;

    public override void Execute()
    {
        // Code to trigger the event
        Debug.Log("event triggered");
    }
}
