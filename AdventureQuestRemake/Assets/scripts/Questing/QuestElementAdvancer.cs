using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestElementAdvancer : MonoBehaviour
{
    public int QuestID, ElementID;
    public bool triggerOnCollision;
    public string notifText = "Quest Updated!";

    public void AdvanceElement()
    {
        QuestManager.instance.AdvanceElement(QuestID, ElementID);
        GameManager.instance.notificationManager.ShowNotification(notifText);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (triggerOnCollision)
        {
            AdvanceElement();
        }
    }

}
