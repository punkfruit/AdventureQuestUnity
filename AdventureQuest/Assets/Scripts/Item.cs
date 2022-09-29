using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{

    public bool sword, staff, bigSword, healthPotion, magicPotion;
    public bool equippable;
    public Sprite iconIMG;
    // public int amount;






    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    public void UseItem()
    {
        Debug.Log("used " + this.name);
    }
}
