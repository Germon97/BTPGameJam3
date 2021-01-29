using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The only audio object in a scene.
/// Accessable via. static functions.
/// </summary>
public class GlobalAudioSource : MonoBehaviour
{
    public static float masterVolume = 1;
    public static float effectsVolume = 1;
    public static float musicVolume = 1;

    public static bool effectsOn = true;
    public static bool musicOn = true;

    private static GlobalAudioSource globalAudio;

    private static AudioSource effectsSource;

    [SerializeField]
    private AudioSource audioSourceForEffects;
    [SerializeField]
    private AudioSource audioSourceForMusic;

    private void Awake()
    {
        // Singleton.
        if (globalAudio != null)
            Destroy(globalAudio);

        globalAudio = this;
        effectsSource = audioSourceForEffects;

        UpdateAudioSetting();
    }

    private void UpdateAudioSetting()
    {
        if (effectsOn)
            audioSourceForEffects.volume = masterVolume * effectsVolume;
        else
            audioSourceForEffects.volume = 0;

        if (musicOn)
            audioSourceForMusic.volume = masterVolume * musicVolume;
        else
            audioSourceForMusic.volume = 0;
    }

    public static void UpdateSetting()
    {
        if (globalAudio != null)
            return;

        globalAudio.UpdateAudioSetting();
    }

    public static void PlaySoundEffect(AudioClip clip)
    {
        if (effectsSource == null)
            return;
        effectsSource.PlayOneShot(clip);
    }
}
