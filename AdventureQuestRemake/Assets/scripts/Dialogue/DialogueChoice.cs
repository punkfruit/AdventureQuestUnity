using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DialogueChoice
{
    public string choiceText;
    public Dialogue nextDialogue; // Reference to the next dialogue if this choice is selected
}

