using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum itemType { greenPotion, redPotion }

[CreateAssetMenu(fileName = "InventoryItem", menuName = "ScriptableObjects/InventoryItem")]
public class InventoryItem : ScriptableObject
{
    public string itemName = "Item";
    public string itemDescription = "Description";
    public Sprite icon;
    public itemType type = itemType.greenPotion;
}
