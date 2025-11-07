using UnityEngine;

public class PushableStatue : MonoBehaviour
{
    [SerializeField] private Rigidbody2D theRB;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip pushSound;
    [SerializeField] private float threshHold = 0.1f;
    [SerializeField] private float stopDelay = 0.2f;

    private float stopTimer;

    void Start()
    {
        if (audioSource && pushSound)
        {
            audioSource.clip = pushSound;
            audioSource.loop = true; // continuous pushing sound
        }
    }

    void Update()
    {
        if (theRB == null || audioSource == null) return;

        if (theRB.linearVelocity.magnitude >= threshHold)
        {
            stopTimer = 0f;
            if (!audioSource.isPlaying)
                audioSource.Play();
        }
        else
        {
            stopTimer += Time.deltaTime;
            if (stopTimer >= stopDelay)
                audioSource.Stop();
        }
    }
}