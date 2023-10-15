using UnityEngine.Audio;
using UnityEngine;

/// <summary>
/// This class is used to clarify the properties of the sounds I introduce into the game.
/// Each sound has a name, volume, pitch, and the audio clip containing the sound
/// </summary>
[System.Serializable]
public class SoundProperties 
{
    public string name;
    
    public AudioClip clip;

    // Makes a slider for volume in the Unity Editor
    [Range(0f, 1f)] 

    public float volume;

    // Makes a slider for pitch in the Unity Editor
    [Range(.1f, 3f)]

    public float pitch;

    [HideInInspector]
    // Creates an audio source for the sound
    public AudioSource source; 
}
