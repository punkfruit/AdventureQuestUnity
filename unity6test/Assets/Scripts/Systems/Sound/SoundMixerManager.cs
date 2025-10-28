using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SoundMixerManager : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer;
    [Header("UI Sliders")]
    [SerializeField] private Slider masterSlider;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;

    private void Start()
    {
        // Load and apply saved volumes
        float master = PlayerPrefs.GetFloat("masterVolume", 1f);
        float music = PlayerPrefs.GetFloat("musicVolume", 1f);
        float sfx = PlayerPrefs.GetFloat("sfxVolume", 1f);

        SetMasterVolume(master);
        SetMusicVolume(music);
        SetSFXVolume(sfx);

        // Update slider visuals to match saved values
        if (masterSlider) masterSlider.value = master;
        if (musicSlider) musicSlider.value = music;
        if (sfxSlider) sfxSlider.value = sfx;
    }

    public void SetMasterVolume(float level)
    {
        level = Mathf.Clamp(level, 0.0001f, 1f);
        audioMixer.SetFloat("masterVolume", Mathf.Log10(level) * 20);
        PlayerPrefs.SetFloat("masterVolume", level);
    }

    public void SetMusicVolume(float level)
    {
        level = Mathf.Clamp(level, 0.0001f, 1f);
        audioMixer.SetFloat("musicVolume", Mathf.Log10(level) * 20);
        PlayerPrefs.SetFloat("musicVolume", level);
    }

    public void SetSFXVolume(float level)
    {
        level = Mathf.Clamp(level, 0.0001f, 1f);
        audioMixer.SetFloat("soundFXVolume", Mathf.Log10(level) * 20);
        PlayerPrefs.SetFloat("sfxVolume", level);
    }
}