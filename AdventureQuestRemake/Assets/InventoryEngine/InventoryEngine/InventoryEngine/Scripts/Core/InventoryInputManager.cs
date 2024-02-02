using UnityEngine;
using UnityEngine.InputSystem;
using MoreMountains.Tools;
using UnityEngine.EventSystems;
using System.Collections.Generic;

namespace MoreMountains.InventoryEngine
{
    public class InventoryInputManager : MonoBehaviour
    {
        public CanvasGroup TargetInventoryContainer;
        public InventoryDisplay TargetInventoryDisplay;


        public InputActionProperty ToggleInventoryKey;
        public InputActionProperty CancelKey;
        public InputActionProperty EquipKey;
        public InputActionProperty UseKey;
        public InputActionProperty UseOrEquipKey;
        public InputActionProperty UnequipKey;
        public InputActionProperty DropKey;
        public InputActionProperty MoveKey;
        public InputActionProperty PrevInvKey;
        public InputActionProperty NextInvKey;




        public GameObject _currentSelection;
        public InventorySlot _currentInventorySlot;
        public InventorySlot CurrentlySelectedInventorySlot;
        public bool InventoryIsOpen = false;
        public bool canOpenInventory = true;

        private void Start()
        {
            if (TargetInventoryContainer != null)
            {
                TargetInventoryContainer.alpha = 0; // Hide inventory on start
                TargetInventoryContainer.blocksRaycasts = false;
            }
        }

        private void Update()
        {
            CheckCurrentlySelectedSlot();
        }

        public void CheckCurrentlySelectedSlot()
        {
            _currentSelection = EventSystem.current.currentSelectedGameObject;
            if(_currentSelection == null)
            {
                return;
            }
            _currentInventorySlot = _currentSelection.gameObject.MMGetComponentNoAlloc<InventorySlot>();
            if(_currentInventorySlot != null)
            {
                CurrentlySelectedInventorySlot = _currentInventorySlot;
            }
        }

        private void OnEnable()
        {
            ToggleInventoryKey.action.Enable();
            CancelKey.action.Enable();
            EquipKey.action.Enable();
            UseKey.action.Enable();
            UseOrEquipKey.action.Enable();
            UnequipKey.action.Enable();
            DropKey.action.Enable();
            MoveKey.action.Enable();
            PrevInvKey.action.Enable();
            NextInvKey.action.Enable();


            ToggleInventoryKey.action.performed += HandleToggleInventory;
            CancelKey.action.performed += HandleCancel;
            EquipKey.action.performed += HandleEquipItem;
            UseKey.action.performed += HandleUseItem;
            UseOrEquipKey.action.performed += HandleUseOrEquipItem;
            UnequipKey.action.performed += HandleUnEquipItem;
            DropKey.action.performed += HandleDropItem;
            MoveKey.action.performed += HandleMoveItem;
            PrevInvKey.action.performed += HandloePreviousInventory;
            NextInvKey.action.performed += HandleNextInventory;


            //Debug.Log("inventory input manager enabled");
        }

        private void OnDisable()
        {
            ToggleInventoryKey.action.Disable();
            CancelKey.action.Disable();
            EquipKey.action.Disable();
            UseKey.action.Disable();
            UseOrEquipKey.action.Disable();
            UnequipKey.action.Disable();
            DropKey.action.Disable();
            MoveKey.action.Disable();
            PrevInvKey.action.Disable();
            NextInvKey.action.Disable();

            //Debug.Log("inventory input manager disabled");
        }

        private void HandleToggleInventory(InputAction.CallbackContext context)
        {
            if (InventoryIsOpen)
            {
                CloseInventory();
            }
            else
            {
                OpenInventory();
            }
        }

        private void HandleCancel(InputAction.CallbackContext context)
        {
            if (InventoryIsOpen)
            {
                CloseInventory();
            }
        }

        private void OpenInventory()
        {
            if (!canOpenInventory)
            {
                return; // Exit the method if dialogue is playing
            }
            MMInventoryEvent.Trigger(MMInventoryEventType.InventoryOpens, null, TargetInventoryDisplay.TargetInventoryName, TargetInventoryDisplay.TargetInventory.Content[0], 0, 0, TargetInventoryDisplay.PlayerID);
            MMGameEvent.Trigger("inventoryOpens");
            InventoryIsOpen = true;
            TargetInventoryContainer.alpha = 1;
            TargetInventoryContainer.blocksRaycasts = true;
            // Add any additional logic for opening the inventory
            Debug.Log("inventory open");

        }

        private void CloseInventory()
        {
            InventoryIsOpen = false;
            TargetInventoryContainer.alpha = 0;
            TargetInventoryContainer.blocksRaycasts = false;
            MMInventoryEvent.Trigger(MMInventoryEventType.InventoryCloses, null, TargetInventoryDisplay.TargetInventoryName, null, 0, 0, TargetInventoryDisplay.PlayerID);
            MMGameEvent.Trigger("inventoryCloses");
            // Add any additional logic for closing the inventory
        }

        // Additional methods...

        private void HandleEquipItem(InputAction.CallbackContext context)
        {
            EquipItem();
        }

        public void EquipItem()
        {
            if (!InventoryIsOpen)
                return;

            if (CurrentlySelectedInventorySlot != null)
            {
                if (CurrentlySelectedInventorySlot.ParentInventoryDisplay.name == "EquipmentInventory")
                {
                    Debug.Log("this is the equipment inventory");
                    return;
                }
                CurrentlySelectedInventorySlot.Equip();
            }
        }

        private void HandleUseItem(InputAction.CallbackContext context)
        {
            UseItem();
        }

        public void UseItem()
        {
            if (!InventoryIsOpen)
                return;

            Debug.Log("use");
            if (CurrentlySelectedInventorySlot != null)
                CurrentlySelectedInventorySlot.Use();
        }

        private void HandleUseOrEquipItem(InputAction.CallbackContext context)
        {
            UseOrEquipItem();
        }

        public void UseOrEquipItem()
        {
            if (CurrentlySelectedInventorySlot == null)
                return;

            if (!InventoryIsOpen)
                return;


            if (CurrentlySelectedInventorySlot.Equippable())
            {
                EquipItem();
            }
            if (CurrentlySelectedInventorySlot.Usable())
            {
                UseItem();
            }
        }

        public void HandleUnEquipItem(InputAction.CallbackContext context)
        {
            UnequipItem();
        }

        public void UnequipItem()
        {
            if (!InventoryIsOpen)
                return;

            Debug.Log("un equip");
            if (CurrentlySelectedInventorySlot != null)
                CurrentlySelectedInventorySlot.UnEquip();
        }

        private void HandleDropItem(InputAction.CallbackContext context)
        {
            DropItem();
        }

        public void DropItem()
        {
            if (!InventoryIsOpen)
                return;

            Debug.Log("drop");
            if (CurrentlySelectedInventorySlot != null)
                CurrentlySelectedInventorySlot.Drop();
        }

        private void HandleMoveItem(InputAction.CallbackContext context)
        {
            MoveItem();
        }

        public void MoveItem()
        {
            if (!InventoryIsOpen)
                return;

            Debug.Log("move");
            if (CurrentlySelectedInventorySlot != null)
                CurrentlySelectedInventorySlot.Move();
        }

        private void HandloePreviousInventory(InputAction.CallbackContext context)
        {
            PreviousInventory();
        }


        public void PreviousInventory()
        {
            if(TargetInventoryDisplay.GoToInventory(-1) != null)
            {
                TargetInventoryDisplay.GoToInventory(-1);
            }
        }

        private void HandleNextInventory(InputAction.CallbackContext context)
        {
            NextInventory();
        }

        public void NextInventory()
        {
            if (TargetInventoryDisplay.GoToInventory(1) != null)
            {
                TargetInventoryDisplay.GoToInventory(1);
            }
        }
    }
}
