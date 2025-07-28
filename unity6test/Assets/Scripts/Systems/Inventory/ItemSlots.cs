using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class ItemSlots : MonoBehaviour, IPointerEnterHandler, ISelectHandler
{
    [SerializeField] Image image;
    [SerializeField] public bool selected = false;
    [SerializeField]
    private Item _item;
    public TextMeshProUGUI amountText;
    public int amount = 0;

    public Button button;

    public int slotIndex;
    private Inventory inventory;

    private void Start()
    {
        inventory = GetComponentInParent<Inventory>();
    }

    public Item Item
    {
        get { return _item; }
        set
        {
            _item = value;

            if(_item == null)
            {
                image.enabled = false;
                amountText.text = string.Empty;
            }
            else
            {
                image.sprite = _item.Icon;
                image.enabled = true;
                UpdateText();
            }
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        UpdateItemInfo();
        inventory.SelectSlot(slotIndex);
    }

    public void OnSelect(BaseEventData eventData)
    {
        UpdateItemInfo();
        inventory?.SelectSlot(slotIndex);
    }

    private void UpdateItemInfo()
    {
       // itemInfoText.text = $"{itemName}\n\n{itemDescription}";
    }


    public void UpdateText()
    {
        if(_item != null)
        {
            if(_item.stackable && amount > 1)
            {
                amountText.text = amount.ToString();
            }
            else
            {
                amountText.text = string.Empty;
            }
        }
    }

    public void UseCurrentItem()
    {
        if(_item != null)
        {
            bool used = _item.UseItem();
            if(used)
            {
                InventoryManager.Instance.RemoveItem();
            }
        }
    }




}
