using System;
using UnityEngine;

[Serializable]
public class AudioManager : MonoBehaviour
{
    private GameObject Camera { get; set; }
    private Audio Jump { get; set; }
    private Audio Coin { get; set; }
    public AudioManagerData Data { get; set; }

    public void InstantiateAudioSource()
    {

    }


    public void Setup()
    {

    }

    private void GetAudioData()
    {

    }

    public void PlayCoinSound()
    {
        //UpdateSourceConfig(coinClip);
        //source.PlayOneShot(coinClip.clip);
    }

    private void UpdateSourceConfig(Audio audio)
    {
        //source.pitch = audio.pitch + UnityEngine.Random.Range(-audio.pitchRange, audio.pitchRange);
        //source.spatialBlend = audio.spatialBlend;
        //source.volume = audio.volume;
        //source.priority = audio.priority;
    }

    public void PlayJumpSound()
    {
        //UpdateSourceConfig(jumpClip);
        //source.PlayOneShot(jumpClip.clip);
    }
}

[Serializable]
public struct Audio
{
    [HideInInspector]
    public AudioSource source;
    public AudioClip clip;
    [Range(-3f, 3f)]
    public float pitch;
    [Range(0f, 1f)]
    public float pitchRange;
    [Range(0, 1f)]
    public float spatialBlend;
    [Range(0, 1f)]
    public float volume;
    [Range(0, 256)]
    public int priority;
}