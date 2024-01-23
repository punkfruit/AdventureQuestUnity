using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelTransition : MonoBehaviour
{
    public string scene;
    public Vector3 nextScenePlayerLocation;


    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            SceneManager.LoadScene(scene);
            PlayerController.instance.transform.position = nextScenePlayerLocation;
        }
    }
}
