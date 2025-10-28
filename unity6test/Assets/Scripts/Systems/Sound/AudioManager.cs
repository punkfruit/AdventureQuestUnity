using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [SerializeField] private AudioSource soundFXObject; // prefab with AudioSource
    [SerializeField, Range(0.8f, 1.2f)] private float minPitch = 0.95f;
    [SerializeField, Range(0.8f, 1.2f)] private float maxPitch = 1.05f;

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
    }

    public void PlaySoundFXClip(AudioClip audioClip, Transform spawnTransform, float volume)
    {
        AudioSource audioSource = Instantiate(soundFXObject, spawnTransform.position, Quaternion.identity);

        audioSource.clip = audioClip;
        audioSource.volume = volume;

        // 🎵 Add pitch variation here
        audioSource.pitch = Random.Range(minPitch, maxPitch);

        audioSource.Play();

        Destroy(audioSource.gameObject, audioSource.clip.length);
    }

    public void PlayRandomSoundFXClip(AudioClip[] audioClips, Transform spawnTransform, float volume)
    {
        int rand = Random.Range(0, audioClips.Length);
        AudioSource audioSource = Instantiate(soundFXObject, spawnTransform.position, Quaternion.identity);

        audioSource.clip = audioClips[rand];
        audioSource.volume = volume;

        // 🎵 Add pitch variation here too
        audioSource.pitch = Random.Range(minPitch, maxPitch);

        audioSource.Play();

        Destroy(audioSource.gameObject, audioSource.clip.length);
    }
}