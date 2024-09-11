using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Dialogue
{
    
    public Sentence[] sentences;
}

[System.Serializable]
public class Sentence
{
    public string characterName;
    public Sprite characterIcon;
    [TextArea(3, 10)]
    public string text;

    public Sentence(string name, Sprite icon, string sentence)
    {
        characterName = name;
        characterIcon = icon;
        text = sentence;
    }
}