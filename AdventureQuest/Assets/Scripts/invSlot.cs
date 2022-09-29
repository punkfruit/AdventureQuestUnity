using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class invSlot : MonoBehaviour
{
    public Image icon;
    public Item itemInSlot;

    public TextMeshProUGUI amountText;
    public GameObject amountTextOBJ; //to enable and disable on a whim







    public void ButtonUseItem()
    {
        if(itemInSlot != null)
            itemInSlot.UseItem();
    }
}
