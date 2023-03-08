using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public enum imageType { sprite, tilemap }
public class SortingLayerChanger : MonoBehaviour
{
    public imageType image_type = imageType.sprite;
    public SpriteRenderer spr;
    public TilemapRenderer tl;
    public float yPos;
    public int above = 10, below = -10;

    public Transform target; //the transform of the object thats being compared against the player.

    private void Update()
    {
        yPos = PlayerController.instance.transform.position.y;


        if(image_type == imageType.sprite)
        {
            if (yPos >= target.transform.position.y)
            {
                spr.sortingOrder = above;
            }
            else
            {
                spr.sortingOrder = below;
            }
        }
       
        if(image_type == imageType.tilemap)
        {
            if (yPos >= target.transform.position.y)
            {
                tl.sortingOrder = above;
            }
            else
            {
                tl.sortingOrder = below;
            }
        }
       
    }
}
