using System;
using UnityEngine;

public class NPC : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip main;
    void Start()
    {
        PlayMainAudio();
    }

    public void StopMainAudio()
    {
        audioSource.Stop();
    }

    public void PlayMainAudio()
    {
        audioSource.clip = main;
        audioSource.loop = true;
        audioSource.spatialBlend = 1f;
        audioSource.Play();
    }
}
