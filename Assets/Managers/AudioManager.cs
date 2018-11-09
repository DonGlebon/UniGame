using System;
using UnityEngine;

[Serializable]
[DefaultExecutionOrder(101)]
public class AudioManager
{

    public AudioData Jump;
    public AudioData Coin;
    public AudioData FootSteps;
    public float footstepsDelay;
    public Camera camera;



    public void Attachlisteners()
    {
        GameManager.Instance.Event.onJump.AddListener(OnJumpListener);
        GameManager.Instance.Event.onCoinPickUp.AddListener(OnCoinPickUpListener);
    }





    private void OnJumpListener()
    {
        Jump.source.Play();
    }

    private void OnCoinPickUpListener(GameObject obj)
    {
        Coin.source.PlayOneShot(Coin.clip);
        GameManager.Instance.DestroyGameObject(obj);
    }

    public void GetCamera()
    {
        camera = Camera.main;
        AddAudioSource(Jump, Coin, FootSteps);
    }

    private void AddAudioSource(params AudioData[] data)
    {
        for (int i = 0; i < data.Length; i++)
        {
            AudioSource source = camera.gameObject.AddComponent<AudioSource>() as AudioSource;
            source.clip = data[i].clip;
            source.pitch = data[i].pitch;
            source.spatialBlend = data[i].spatialBlend;
            source.volume = data[i].volume;
            source.priority = data[i].priority;
            data[i].source = source;
        }
    }

}

[Serializable]
public class AudioData
{
    [HideInInspector]
    public AudioSource source;
    public AudioClip clip;
    [Range(-3f, 3f)]
    public float pitch = 1f;
    [Range(0f, 1f)]
    public float pitchRange;
    [Range(0, 1f)]
    public float spatialBlend;
    [Range(0, 1f)]
    public float volume;
    [Range(0, 256)]
    public int priority;
}