using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

using UnityEngine.UI;
using static UnityEditor.Progress;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager instance;
    public Inventory inventory;

    [SerializeField] public List<Item> items;

    public GameObject inventoryPanel;
    public Button initialInventoryButton;
    public Inventory _inventoryPanel;
    public bool inventoryOpen = false;
    public PlayerInput playerInput;

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

    private void Start()
    {
        if (playerInput != null)
        {
            playerInput.actions["OpenInventory"].performed += OnInventoryOpenPerformed;
            playerInput.actions["CloseInventory"].performed += OnInventoryClosedPerformed;
            //playerInput.actions["Cancel"].performed += OnCancelPerformed;
        }
    }

    private void OnInventoryOpenPerformed(InputAction.CallbackContext context)
    {
        OpenInventory();

    }

    private void OnInventoryClosedPerformed(InputAction.CallbackContext context)
    {

        CloseInventory();

    }

    public void OpenInventory()
    {
        if (!DialogueManager.instance.dialogueIsPlaying)
        {
            inventoryPanel.SetActive(true);
            playerInput.SwitchCurrentActionMap("UI");
            _inventoryPanel.DeselectSlot();
            initialInventoryButton.Select();

            inventoryOpen = true;
        }

    }

    public void CloseInventory()
    {
        inventoryPanel.SetActive(false);
        _inventoryPanel.DeselectSlot();
        playerInput.SwitchCurrentActionMap("Player");

        inventoryOpen = false;
    }

    public bool AddItem(Item item)
    {
        for (int i = 0; i < inventory.itemSlots.Length; i++)
        {
            if (inventory.itemSlots[i].Item == null)
            {
                inventory.itemSlots[i].Item = item;
                inventory.RefreshUI();
                Debug.Log("found empty slot");
                return true;
            }
            else
            {
                if (inventory.itemSlots[i].Item == item && inventory.itemSlots[i].Item.stackable)
                {
                    inventory.itemSlots[i].amount += 1;
                    inventory.RefreshUI();
                    Debug.Log("found stackable slot");
                    return true;
                }
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
        if (inventory.itemSlots[selected].amount <= 0)
        {
            inventory.itemSlots[selected].Item = null;
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
}
