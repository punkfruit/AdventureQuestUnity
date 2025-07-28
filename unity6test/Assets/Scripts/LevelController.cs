using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelController : MonoBehaviour
{

    public bool setBounds = false;
    public Collider2D boundsToSet;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (setBounds)
        {
            CameraController.instance.UpdateConfiner(boundsToSet);
        }

        SaveManager.Instance.CurrentData.currentScene = SceneManager.GetActiveScene().name;
    }

    
}
