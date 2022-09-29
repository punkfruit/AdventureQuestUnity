using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static Inventory instance;

    public List<Item> items;
    public invSlot[] slots;
    public GameObject inventory;
    public Color blank;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        inventory.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Inventory"))
        {
            if (inventory.activeInHierarchy)
            {
                inventory.SetActive(false);
            }
            else
            {
                inventory.SetActive(true);
                UpdateInventory();
            }
        }
    }


    public void UpdateInventory()
    {
       for(int i = 0; i < slots.Length; i++)
        {
            slots[i].icon.color = blank;
            slots[i].amountTextOBJ.SetActive(false);
        }

        for (int i = 0; i < items.Count; i++)
        {
            slots[i].itemInSlot = items[i];
            
            slots[i].icon.color = Color.white;
            slots[i].icon.sprite = slots[i].itemInSlot.iconIMG;
                //slots[i].amountText.text = slots[i].itemInSlot.amount.ToString();
        }


    }

    public bool AddItem(Item itemToAdd)
    {
        if (items.Count + 1 > slots.Length)
        {
            return true;
        }
        else
        {
            items.Add(itemToAdd);
            return false;
        }
        
        //UpdateInventory();
    }
}
