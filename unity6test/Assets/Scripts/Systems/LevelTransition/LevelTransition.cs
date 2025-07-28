using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelTransition : MonoBehaviour
{
    public enum Side { Left, Right, Top, Bottom }
    public string levelName; // Name of the scene to load
    public string targetTransitionArea = "LevelTransition"; // Target transition area name in the next scene
    public bool centerPlayer = false; // Center player on the transition point
    public Side side = Side.Left;
    public Vector2 positionOffset = new Vector2(32, 0); // Default offset for placement

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Vector2 offset = GetOffset(collision.transform.position);
            LevelManager.Instance.LoadNewLevel(levelName, targetTransitionArea, offset);
        }
    }

    private Vector2 GetOffset(Vector2 playerPos)
    {
        Vector2 offset = Vector2.zero;

        if (side == Side.Left || side == Side.Right)
        {
            offset.x = side == Side.Left ? -positionOffset.x : positionOffset.x;
            offset.y = centerPlayer ? 0 : playerPos.y - transform.position.y;
        }
        else
        {
            offset.y = side == Side.Top ? -positionOffset.y : positionOffset.y;
            offset.x = centerPlayer ? 0 : playerPos.x - transform.position.x;
        }

        return offset;
    }
}
