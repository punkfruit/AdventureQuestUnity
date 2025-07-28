using UnityEngine;

public class PickupItem : MonoBehaviour
{
    public Item item;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            bool i = InventoryManager.Instance.AddItem(item);
            if (i) //if it was succesfully picked up
            {
                Destroy(gameObject);
                //play pickup sound
            }
            else
            {
                //play some kind of reject sound
            }
        }
    }
}
