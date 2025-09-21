using UnityEngine;

public class Chest : MonoBehaviour
{
    public Interactable baseInteractable;
    public Sprite closed, open;
    public SpriteRenderer spr;

    public Item[] potentialItems;
    private Item finalItem;

    public Animator anim;
    public SpriteRenderer animSpr;

    public void OpenChest()
    {
       

        if (baseInteractable.state == false) //if the chest is closed
        {
            int chance = (int)Random.Range(0, potentialItems.Length);
            finalItem = potentialItems[chance];

            if (InventoryManager.Instance.AddItem(finalItem))
            {
                animSpr.sprite = finalItem.Icon;
                spr.sprite = open;
                anim.SetTrigger("Open");
                //play sound later
                baseInteractable.indicator.SetActive(false);
                baseInteractable.state = true;
                SaveManager.Instance.SetObjectState(baseInteractable.uniqueID, baseInteractable.state);
            }
            else
            {
                Debug.Log("inventory full, chest cant be opened");
            }

        }
        
    }


    public void ApplyState() //listening to the base interactable apply state event
    {
        if(baseInteractable.state == true)
        {
            spr.sprite = open;
        }
        else
        {
            spr.sprite = closed;
        }
    }

}
