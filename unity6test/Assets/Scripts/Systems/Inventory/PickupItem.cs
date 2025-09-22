using UnityEngine;
using UnityEngine.Events;

public class PickupItem : MonoBehaviour
{
    public Item item;
    public UnityEvent OnPickup;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            bool i = InventoryManager.Instance.AddItem(item);
            if (i) //if it was succesfully picked up
            {
                OnPickup.Invoke();
                Destroy(gameObject);
                //play pickup sound
            }
            else
            {
                //play some kind of reject sound
            }
        }
    }

    public void ApplyState(bool state)
    {
        if (state)
        {
            //Debug.Log("item already picked up");
            Destroy(gameObject);
        }
    }
}
