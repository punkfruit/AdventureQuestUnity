using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    [SerializeField] Transform itemsParent;
    [SerializeField] public ItemSlots[] itemSlots;
    [SerializeField] public int selectedSlot = -1;

    public GameObject infoBox;
    public TextMeshProUGUI nameText, descriptionText;
    [SerializeField] public Image infoBoxIcon;

    // [SerializeField] GameObject contextMenu, cMenu1, cMenu2, cMenu3;
    public InventoryManager inventoryManager;

    private void OnValidate()
    {
        if(itemsParent != null)
            itemSlots = itemsParent.GetComponentsInChildren<ItemSlots>();

        RefreshUI();
    }

    public void RefreshUI()
    {
        for(int i = 0; i < itemSlots.Length; i++)
        {
            itemSlots[i].UpdateText();
        }
    }



    public void SelectSlot(int num)
    {
        selectedSlot = num;
        itemSlots[num].button.Select();
        for (int i = 0; i < itemSlots.Length; ++i)
        {
            itemSlots[i].selected = false;
        }

        if (itemSlots[num].Item != null)
        {
            itemSlots[num].selected = true;

            infoBox.SetActive(true);
            nameText.text = itemSlots[num].Item.name;
            descriptionText.text = itemSlots[num].Item.ItemDescription;
            infoBoxIcon.sprite = itemSlots[num].Item.Icon;

        }
        else
        {
            nameText.text = "";
            descriptionText.text = "";
            infoBoxIcon.sprite = null;
            infoBox.SetActive(false);
        }

    }

    public void DeselectSlot()
    {
        for (int i = 0; i < itemSlots.Length; ++i)
        {
            itemSlots[i].selected = false;
        }

        nameText.text = "";
        descriptionText.text = "";
        infoBoxIcon.sprite = null;

        if (selectedSlot != -1)
            itemSlots[selectedSlot].button.Select();

        selectedSlot = -1;
        infoBox.SetActive(false);
    }

    public void UseSelectedItem()
    {
        if (itemSlots[selectedSlot].Item != null && selectedSlot != -1)
            itemSlots[selectedSlot].UseCurrentItem();
    }


    
}
