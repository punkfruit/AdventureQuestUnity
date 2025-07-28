using UnityEngine;

public enum type { usable, equipable, craftable }

[CreateAssetMenu]
public class Item : ScriptableObject
{
    public int id;
    public string ItemName;
    [TextArea(3,10)]
    public string ItemDescription;
    public Sprite Icon;
    public type itemType;
    public bool stackable = false;
    public int maxStackSize = 1;

    public virtual bool UseItem()
    {
        Debug.Log("used item " + this);
        return true;
    }

}
