using System;
using UnityEngine;
using UnityEngine.Events;

public class PressurePlate : MonoBehaviour
{
    public Sprite pressed, unpressed;
    public UnityEvent OnPressurePlatePressed, OnPressurePlateUnpressed;
    public AudioClip pressSound, unpressSound;

    public int thingsOnPlate; //public to see in inspector

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            thingsOnPlate++;
            
            
            
            if (thingsOnPlate == 1)
            {
                PressPlate();
            }
        }

        
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            thingsOnPlate--;
            
            
            if (thingsOnPlate <= 0)
            {
                UnpressPlate();
            }
        }

        
    }

    public void PressPlate()
    {
        OnPressurePlatePressed?.Invoke();
        GetComponent<SpriteRenderer>().sprite = pressed;
        AudioManager.instance.PlaySoundFXClip(pressSound, transform, 1f);
    }

    public void UnpressPlate()
    {
        OnPressurePlateUnpressed?.Invoke();
        GetComponent<SpriteRenderer>().sprite = unpressed;
        AudioManager.instance.PlaySoundFXClip(unpressSound, transform, 1f);
    }
}
