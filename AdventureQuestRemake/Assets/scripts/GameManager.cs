using MoreMountains.Tools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public InputActionAsset inputActions;
    // Start is called before the first frame update

    private void Awake()
    {
        if(instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
    }
    void Start()
    {
        DontDestroyOnLoad(this);


        MMEventManager.TriggerEvent(new MMGameEvent("Load"));

    }

    // Update is called once per frame
    void Update()
    {
       
    }
}
