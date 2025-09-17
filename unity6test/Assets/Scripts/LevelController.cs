using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelController : MonoBehaviour
{

    public bool setBounds = false;
    public Collider2D boundsToSet;
    public string chapter = "Chapter 1";


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (setBounds)
        {
            CameraController.instance.UpdateConfiner(boundsToSet);
        }

        SaveManager.Instance.CurrentData.currentScene = SceneManager.GetActiveScene().name;
        GameManager.instance.CurrentLevelController = this;
    }

    
}
