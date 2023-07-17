using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager instance;

    public bool inventoryOpen = false;
    public Sprite[] icons;
    public InventorySlot[] slots;
    public GameObject inventoryScreen;
    // Start is called before the first frame update
    private void Awake()
    {
        if(instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
    }

    private void Start()
    {
        inventoryScreen.SetActive(inventoryOpen);
        UpdateInventory();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CloseInventory() 
    {
        inventoryOpen = false;
        inventoryScreen.SetActive(false);
        PlayerController.instance.canMove = true;
        PlayerController.instance.canSwing = true;
    }

    public void ToggleInventory(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (inventoryOpen)
            {
                inventoryOpen = false;
                inventoryScreen.SetActive(false);
                PlayerController.instance.canMove = true;
                PlayerController.instance.canSwing = true;
            }
            else
            {
                inventoryOpen = true;
                inventoryScreen.SetActive(true);
                PlayerController.instance.canMove = false;
                PlayerController.instance.canSwing = false;
            }
        }
    }

    public void UpdateInventory()
    {
        for(int i = 0; i < slots.Length; i ++)
        {
            slots[i].UpdateSlot();
        }
    }
}
