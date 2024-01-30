using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelTransition : MonoBehaviour
{
    public string scene;
    public Vector3 nextScenePlayerLocation;

    [Space]
    public DialogueTrigger TriggerToDisable; //if theres a dialogue trigger like a sign next to the transition it should be disabled before the transition occurs


    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            CameraController.instance.MoveCam();
            SceneManager.LoadScene(scene);
            PlayerController.instance.transform.position = nextScenePlayerLocation;

            if(TriggerToDisable != null)
            {
                TriggerToDisable.DisableTrigger();
            }
            
        }
    }
}
