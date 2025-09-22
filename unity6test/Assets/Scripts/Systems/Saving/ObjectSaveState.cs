using System;
using UnityEngine;
using UnityEngine.Events;

public class ObjectSaveState : MonoBehaviour
{
    public string uniqueID;
    public bool state = false; //default state, ie chest being closed
    public UnityEvent<bool> ApplyStateEvent;

    private void Start()
    {
        state = SaveManager.Instance.GetObjectState(uniqueID);
        ApplyState(state);
    }
    
    public void ApplyState(bool state)
    {
        // Replace this with logic that reflects the state, like animation or enabling/disabling a collider
        ApplyStateEvent?.Invoke(state);
        Debug.Log($"Applying state to {uniqueID}: {state}");

    }

    public void setState(bool stateToSet)
    {
        state = stateToSet;
        SaveManager.Instance.SetObjectState(uniqueID, state);
    }
}
