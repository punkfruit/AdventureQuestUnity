using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
//using static UnityEditor.Progress;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;
    public Inventory inventory;

    [SerializeField] public List<Item> items;
    public int[] itemAmount; //matches the items list in length, each int represents the amount of items in that item slot

    public List<Item> allItems; //every item that should be in the game, hopefully i remember to add them all!

    private void Awake()
    {
        if(Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    private void Start()
    {
        for(int i = 0; i < inventory.itemSlots.Length; i++)
        {
            items.Add(null);
            SaveManager.Instance.CurrentData.inventoryItems.Add(-1); //null not being allowed is lame, hopefully -1 works lol
        }

        itemAmount = new int[items.Count];
        SaveManager.Instance.CurrentData.inventoryItemCount = new int[items.Count];
    }

    public bool AddItem(Item item)
    {
        for (int i = 0; i < inventory.itemSlots.Length; i++)
        {
            if (inventory.itemSlots[i].Item == item && inventory.itemSlots[i].Item.stackable && inventory.itemSlots[i].amount < inventory.itemSlots[i].Item.maxStackSize)
            {
                inventory.itemSlots[i].amount += 1;
                itemAmount[i]++;
                SaveManager.Instance.CurrentData.inventoryItemCount[i] = itemAmount[i];
                inventory.RefreshUI();
                Debug.Log("found stackable slot");
                return true;
            }
            else if(inventory.itemSlots[i].Item == null)
            {

                inventory.itemSlots[i].Item = item;
                items[i] = item;
                itemAmount[i]++;
                SaveManager.Instance.CurrentData.inventoryItems[i] = item.id;
                SaveManager.Instance.CurrentData.inventoryItemCount[i] = itemAmount[i];
                inventory.RefreshUI();
                Debug.Log("found empty slot");
                return true;

            }


        }

        Debug.Log("didnt find slot");
        return false;
    }


    public bool IsFull()
    {
        for (int i = 0; i < inventory.itemSlots.Length; i++)
        {
            if (inventory.itemSlots[i].Item == null)
            {
                return false;
            }
        }

        return true;
    }

    public void DropItem()
    {
        RemoveItem();
        //make a pickup spawn later
    }

    public void RemoveItem()
    {
        int selected = inventory.selectedSlot;

        inventory.itemSlots[selected].amount -= 1;
        itemAmount[selected]--;
        SaveManager.Instance.CurrentData.inventoryItemCount[selected] = itemAmount[selected];
        if (inventory.itemSlots[selected].amount <= 0)
        {
            inventory.itemSlots[selected].Item = null;
            items[selected] = null;
            SaveManager.Instance.CurrentData.inventoryItems[selected] = -1;
            inventory.DeselectSlot();
        }

        inventory.RefreshUI();
    }

    public bool RemoveSpecificItem(Item item)
    {
        for (int i = 0; i < inventory.itemSlots.Length; i++)
        {
            if (inventory.itemSlots[i].Item == item && !inventory.itemSlots[i].Item.stackable)
            {
                inventory.itemSlots[i].Item = null;
                inventory.RefreshUI();
                return true;
            }
            else if (inventory.itemSlots[i].Item == item && inventory.itemSlots[i].Item.stackable)
            {
                inventory.itemSlots[i].amount -= 1;
                if (inventory.itemSlots[i].amount <= 0)
                {
                    inventory.itemSlots[i].Item = null;
                }

                inventory.RefreshUI();
                return true;
            }
        }

        return false;
    }

    public void ClearInventory()
    {
        for (int i = 0; i < inventory.itemSlots.Length; i++)
        {
            items[i] = null;
            itemAmount[i] = 0;
            inventory.itemSlots[i].Item = null;
        }
    }

    public void LoadInventoryByID(List<int> IDs, int[] amount)
    {
        ClearInventory();
        for (int i = 0; i < inventory.itemSlots.Length; i++)
        {
            if (IDs[i] == -1)
            {
                items[i] = null;
                itemAmount[i] = 0;
                inventory.itemSlots[i].Item = null;
            }
            else
            {
                items[i] = allItems[IDs[i]];
                itemAmount[i] = amount[i];
                inventory.itemSlots[i].Item = items[i];
                inventory.itemSlots[i].amount = amount[i];
            }


            inventory.RefreshUI();
        }

        
    }
}
