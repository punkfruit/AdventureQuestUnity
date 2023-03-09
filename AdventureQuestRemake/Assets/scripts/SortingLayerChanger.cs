using UnityEngine;
using UnityEngine.Tilemaps;

public enum ImageType { Sprite, Tilemap }

public class SortingLayerChanger : MonoBehaviour
{
    public ImageType imageType = ImageType.Sprite;
    public SpriteRenderer spriteRenderer;
    public TilemapRenderer tilemapRenderer;
    public float yPos;
    public int above = 10;
    public int below = -10;
    public Transform target;

    private void Update()
    {
        yPos = PlayerController.instance.transform.position.y;

        if (imageType == ImageType.Sprite)
        {
            if (yPos >= target.position.y)
            {
                spriteRenderer.sortingOrder = above;
            }
            else
            {
                spriteRenderer.sortingOrder = below;
            }
        }
        else if (imageType == ImageType.Tilemap)
        {
            if (yPos >= target.position.y)
            {
                tilemapRenderer.sortingOrder = above;
            }
            else
            {
                tilemapRenderer.sortingOrder = below;
            }
        }
    }
}
