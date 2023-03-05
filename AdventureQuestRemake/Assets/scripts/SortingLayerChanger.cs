using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SortingLayerChanger : MonoBehaviour
{
    public SpriteRenderer spr;
    public float yPos;

    private void Update()
    {
        yPos = PlayerController.instance.transform.position.y;

        if(yPos >= transform.position.y)
        {
            spr.sortingOrder = 10;
        }
        else
        {
            spr.sortingOrder = -10;
        }
    }
}
