using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundMixerManager : MonoBehaviour
{
    public static SoundMixerManager instance;

    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private AudioSource soundFXObject;

    private void Awake()
    {
        instance = this;
    }

    public void SetMasterVolume(float level)
    {
        audioMixer.SetFloat("masterVolume", level);
    }

    public void SetSoundFXVolume(float level)
    {
        audioMixer.SetFloat("soundFXVolume", level);
    }

    public void SetMusicVolume(float level)
    {
        audioMixer.SetFloat("musicVolume", level);
    }

    public void PlaySoudFXClip(AudioClip audioClip, Transform spawnTransform, float volume)
    {
        AudioSource audioSource = Instantiate(soundFXObject, spawnTransform.position, Quaternion.identity);

        audioSource.clip = audioClip;

        audioSource.volume = volume;

        audioSource.Play();

        float clipLength = audioSource.clip.length;

        Destroy(audioSource.gameObject, clipLength);
    }
}
