using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    public Image image;
    public InventoryItem item;
    public string itemName = "test item";
    public string itemDescription = "this is a test item";


    public void UpdateSlot()
    {
        if(item != null)
        {
            itemName = item.itemName;
            itemDescription = item.itemDescription;
            image.sprite = item.icon;
        }
        else
        {
            itemName = "";
            itemDescription = "";
            image.sprite = null;
        }
        

    }
}
