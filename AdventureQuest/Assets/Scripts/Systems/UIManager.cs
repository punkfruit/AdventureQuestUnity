using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    public Slider healthSlider;

    public PlayerInput playerInput;

    public GameObject inventoryPanel;
    public Button initialInventoryButton;
   // public Inventory _inventoryPanel;
    public bool inventoryOpen = false;

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
        healthSlider.maxValue = PlayerController.instance.maxhealth;
        healthSlider.value = PlayerController.instance.health;
        playerInput = PlayerController.instance.playerInput;
        if (playerInput != null)
        {
            playerInput.actions["OpenInventory"].performed += OnInventoryOpenPerformed;
            playerInput.actions["CloseInventory"].performed += OnInventoryClosedPerformed;
            playerInput.actions["Cancel"].performed += OnCancelPerformed;
        }
    }

    // Update is called once per frame
    

    public void UpdateHealthSlider()
    {
        healthSlider.value = PlayerController.instance.health;
    }


    private void OnInventoryOpenPerformed(InputAction.CallbackContext context)
    {
        OpenInventory();

    }

    private void OnInventoryClosedPerformed(InputAction.CallbackContext context)
    {

        CloseInventory();

    }

    private void OnCancelPerformed(InputAction.CallbackContext context)
    {
        /*
        if (_inventoryPanel.cMenuOpen())
        {
            _inventoryPanel.DeselectSlot();
        }
        else
        {
            CloseInventory();
        }
        */
    }
    public void CloseInventory()
    {
        inventoryPanel.SetActive(false);
       // _inventoryPanel.DeselectSlot();
        playerInput.SwitchCurrentActionMap("Player");

        inventoryOpen = false;
    }

    public void OpenInventory()
    {
        if(!DialogueManager.instance.dialogueIsPlaying)
        {
            inventoryPanel.SetActive(true);
            playerInput.SwitchCurrentActionMap("UI");
          //  _inventoryPanel.DeselectSlot();
            initialInventoryButton.Select();

            inventoryOpen = true;
        }
        
    }
}
