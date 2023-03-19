using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoofHider : MonoBehaviour
{
    public Animator anim;


    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            anim.SetBool("playerInside", true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            anim.SetBool("playerInside", false);
        }
    }
}
