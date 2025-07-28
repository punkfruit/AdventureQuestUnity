using System;
using UnityEngine;

public class OneOffEffect : MonoBehaviour
{
    private void Start()
    {
        //play sound maybe later
    }

    public void SelfDestruct()
    {
        Destroy(gameObject);
    }
}
