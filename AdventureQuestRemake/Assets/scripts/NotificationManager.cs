using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NotificationManager : MonoBehaviour
{
    public Animator anim;
    public TextMeshProUGUI notifText;


    public void ShowNotification(string text)
    {
        notifText.text = text;
        anim.SetTrigger("ActivateNotif");
    }
}
