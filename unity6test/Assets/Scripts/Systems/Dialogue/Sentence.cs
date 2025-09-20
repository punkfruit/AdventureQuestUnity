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
