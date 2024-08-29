using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public enum ImageTypes { Sprite, Tilemap }
public class Ysort : MonoBehaviour
{
    public ImageTypes imageType = ImageTypes.Sprite;
    public SpriteRenderer spriteRenderer;
    public TilemapRenderer tilemapRenderer;
    
    public Transform target;

    private void Update()
    {
        if (imageType == ImageTypes.Sprite)
        {
            spriteRenderer.sortingOrder = -(int)(target.transform.position.y * 100f);
        }
        else if (imageType == ImageTypes.Tilemap)
        {   
            tilemapRenderer.sortingOrder = -(int)(target.transform.position.y * 100f);
        }
    }
}
