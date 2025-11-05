using UnityEngine;

public class BarredDoor : MonoBehaviour
{
    public Collider2D doorCollider;
    public Animator anim;
    public AudioClip doorSound;


    public void OpenDoor()
    {
        anim.SetBool("Open", true);
        doorCollider.enabled = false;
        AudioManager.instance.PlaySoundFXClip(doorSound, transform, 1f);
    }

    public void CloseDoor()
    {
        anim.SetBool("Open", false);
        doorCollider.enabled = true;
        AudioManager.instance.PlaySoundFXClip(doorSound, transform, 1f);
    }
}
