using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New SentenceWithChoicesa", menuName = "Dialogue/SentenceWithChoices")]
public class SentenceWithChoices : Sentence
{
    public DialogueChoice[] choices;

    

    public override void Execute()
    {
        // Logic to display choices (if any)
    }
}
