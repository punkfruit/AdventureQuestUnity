using UnityEngine;

public class PushableStatue : MonoBehaviour
{

    public Rigidbody2D theRB;
    public AudioSource audioSource;
    public AudioClip pushSound;

    public float threshHold = 0.1f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        audioSource.clip = pushSound;
    }

    // Update is called once per frame
    void Update()
    {
        if (theRB != null)
        {
            if (theRB.linearVelocityX >= threshHold || theRB.linearVelocityY >= threshHold || theRB.linearVelocityX <= -threshHold || theRB.linearVelocityY <= -threshHold)
            {
                if(!audioSource.isPlaying)
                    audioSource.Play();
            }
            else
            {
                audioSource.Stop();
            }
        }
    }
}
