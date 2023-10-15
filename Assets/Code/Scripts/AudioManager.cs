using UnityEngine.Audio;
using UnityEngine;
using System;

public class AudioManager : MonoBehaviour
{
    // This is the array of sounds, using the serializable class SoundProperties
    public SoundProperties[] sounds;

    /// <summary>
    /// This method matchs the Parameter name to the sound name in the array of sounds
    /// It then plays the sound once matched.
    /// </summary>
    /// <param name="name"></param>
    public void Play(string name) 
    {
        SoundProperties s = Array.Find(sounds, sound => sound.name == name);

        s.source.Play();
    }

    /// <summary>
    /// This method creates an AudioSource for each sound inside the sounds array
    /// </summary>
    void Awake()
    {
        DontDestroyOnLoad(gameObject);

        foreach (SoundProperties s in sounds)
        {
            // Creates an AudioSource for each sound in the array
            CreateAudioSourceAndSoundAttributes(s);
        }
    }

    /// <summary>
    /// This method creates an AudioSource.
    /// It then creates sound attributes based on the sound in the sound array
    /// </summary>
    /// <param name="s"></param>
    private void CreateAudioSourceAndSoundAttributes(SoundProperties s)
    {
        s.source = gameObject.AddComponent<AudioSource>();
        s.source.clip = s.clip;
        s.source.volume = s.volume;
        s.source.pitch = s.pitch;
    }
}
