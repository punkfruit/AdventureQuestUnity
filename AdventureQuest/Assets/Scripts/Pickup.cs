using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{

    public bool playerInRange;
    public Item item;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    void Update()
    {
        if (playerInRange)
        {
            if (Input.GetButtonDown("Action"))
            {
                bool invFull = Inventory.instance.AddItem(item);
                if (invFull)
                {
                    Debug.Log("inventory full!");
                }else
                {
                    Debug.Log("pickep up " + item.name);
                    Destroy(gameObject);
                }
            }
        }
    }

    

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            playerInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            playerInRange = false;

        }
    }
}
