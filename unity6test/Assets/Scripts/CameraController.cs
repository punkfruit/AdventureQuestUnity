using UnityEngine;
using System.Collections;
using Unity.Cinemachine;

public class CameraController : MonoBehaviour
{

    public static CameraController instance;


    public CinemachineCamera Camera;
    public CinemachineConfiner2D Confiner;
    public CinemachinePositionComposer PositionComposer;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }

        //dont destroy on load is called on parent "Managers" game object
    }


    public void UpdateConfiner(Collider2D bounds) //keeps camera from moving out of set bounds
    {
        Confiner.BoundingShape2D = bounds;
        Debug.Log("bounds set");
    }

    public void ChangeTarget(Transform tran) //called by the player in their start function
    {
        Camera.Follow = tran;
    }


}
