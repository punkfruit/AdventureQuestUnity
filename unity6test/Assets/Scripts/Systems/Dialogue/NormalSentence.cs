using UnityEngine;

[CreateAssetMenu(fileName = "New Sentence", menuName = "Dialogue/Sentence")]
public class NormalSentence : Sentence
{
    public override void Execute()
    {
        Debug.Log("sentence executed code");
    }
}
