using UnityEngine;
using UnityEngine.Tilemaps;

public enum Type {SPRITE, TILEMAP}
public class YSort : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private TilemapRenderer tilemapRenderer;
    public Transform sortTransform;
    public int offset = 0;
    public Type sortType = Type.SPRITE;

    private void Awake()
    {
        if(sortType == Type.SPRITE) 
            spriteRenderer = GetComponent<SpriteRenderer>();
        
        if(sortType == Type.TILEMAP)
            tilemapRenderer = GetComponent<TilemapRenderer>();
        
        if (sortTransform == null)
            sortTransform = transform;
        
        UpdateSortingOrder();
    }

    private void UpdateSortingOrder()
    {
        // Use the y position of the object to determine the sorting order
        if(sortType == Type.SPRITE) 
            spriteRenderer.sortingOrder = Mathf.RoundToInt(-sortTransform.position.y * 100 + offset);
        
        if(sortType == Type.TILEMAP)
            tilemapRenderer.sortingOrder = Mathf.RoundToInt(-sortTransform.position.y * 100 + offset);
    }

    private void OnValidate()
    {
        // Ensure sorting order updates in the editor when objects are moved
        UpdateSortingOrder();
    }

    private void LateUpdate()
    {
        // Update sorting order only if the object has moved significantly
        if (sortTransform.hasChanged)
        {
            UpdateSortingOrder();
            transform.hasChanged = false;
        }
    }
}