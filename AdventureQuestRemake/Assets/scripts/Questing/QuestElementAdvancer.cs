using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestElementAdvancer : MonoBehaviour
{
    public int QuestID, ElementID;
    public bool triggerOnCollision;


    public void AdvanceElement()
    {

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (triggerOnCollision)
        {
            AdvanceElement();
        }
    }

}
