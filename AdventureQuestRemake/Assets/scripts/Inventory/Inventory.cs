using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    //[SerializeField] List<Item> items;
    [SerializeField] Transform itemsParent;
    [SerializeField] public InventorySlot[] itemSlots;
    [SerializeField] public int selectedSlot = -1;


    //[SerializeField] GameObject contextMenu, cMenu1, cMenu2, cMenu3;
    public InventoryManager inventoryManager;


    private void OnValidate()
    {
        if (itemsParent != null)
            itemSlots = itemsParent.GetComponentsInChildren<InventorySlot>();

        RefreshUI();
    }


    public void RefreshUI()
    {
        /*
        int i = 0;
        for (; i < inventoryManager.items.Count && i < InventorySlot.Length; i++)
        {
            InventorySlot[i].Item = inventoryManager.items[i];
        }

        for(; i < InventorySlot.Length; ++i)
        {
            InventorySlot[i].Item = null;
        }
        */

        for (int i = 0; i < itemSlots.Length; i++)
        {
            itemSlots[i].UpdateText();
        }
    }

    /*
    public void MoveContextMenu()
    {
        for (int i = 0; i < itemSlots.Length; ++i)
        {
            if (itemSlots[i].selected)
                contextMenu.transform.position = itemSlots[i].transform.position;
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
            MoveContextMenu();
            contextMenu.SetActive(true);

            switch (itemSlots[num].Item.itemType)
            {
                case type.usable:
                    cMenu1.SetActive(true);
                    cMenu2.SetActive(false);
                    cMenu3.SetActive(false);
                    break;
                case type.equipable:
                    cMenu1.SetActive(false);
                    cMenu2.SetActive(true);
                    cMenu3.SetActive(false);
                    break;
                case type.craftable:
                    cMenu1.SetActive(false);
                    cMenu2.SetActive(false);
                    cMenu3.SetActive(true);
                    break;
            }
        }
        else
        {
            //contextMenu.SetActive(false);
            cMenu1.SetActive(false);
            cMenu2.SetActive(false);
            cMenu3.SetActive(false);
        }


    }
    */

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
        }
    }



    public void DeselectSlot()
    {
        for (int i = 0; i < itemSlots.Length; ++i)
        {
            itemSlots[i].selected = false;
        }
        //contextMenu.SetActive(false);
        //cMenu1.SetActive(false);
        //cMenu2.SetActive(false);
        //cMenu3.SetActive(false);

        if (selectedSlot != -1)
            itemSlots[selectedSlot].button.Select();

        selectedSlot = -1;
    }

    public void UseSelectedItem()
    {
        itemSlots[selectedSlot].UseCurrentItem();
    }

    /*
    public bool cMenuOpen()
    {


        if (cMenu1.activeInHierarchy)
        {
            return true;
        }
        else if (cMenu2.activeInHierarchy)
        {
            return true;
        }
        else if (cMenu3.activeInHierarchy)
        {
            return true;
        }
        else
        {
            return false;
        }

    }
    */
}
