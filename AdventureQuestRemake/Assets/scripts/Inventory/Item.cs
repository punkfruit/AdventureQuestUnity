using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum type { usable, equipable, craftable }

[CreateAssetMenu]
public class Item : ScriptableObject
{
    public string ItemName;
    public Sprite Icon;
    public type itemType;
    public bool stackable = false;
    public int maxStackSize = 1;
    public Color color = Color.white;



    public virtual bool UseItem()
    {
        Debug.Log("used item" + this);
        return true;
    }
}
