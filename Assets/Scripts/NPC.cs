using System;
using UnityEngine;

public class NPC : MonoBehaviour
{
    public string characterName;
    public AnimationClip idleAnimation;
    public AudioSource audioSource;
    public AudioClip main;
    public Dialogue dialogue;
    public GameObject npcFocusPoint;
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

    public void StartDialogue()
    {
        dialogue.StartDialogue(this.gameObject);
    }
}
