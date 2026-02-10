using UnityEngine;

public class SoundController : MonoBehaviour
{
    public static SoundController instance;
    public AudioSource musicAudioSource;
    public AudioSource ambientAudioSource;
    void Awake()
    {
        instance = this;
    }
    public void StopAllSound()
    {
        if (ambientAudioSource) musicAudioSource.Stop();
        if (ambientAudioSource) ambientAudioSource.Stop();
    }
    public void PlayMusicClip(AudioClip clip)
    {
        PlayAudioClip(clip, musicAudioSource);
    }
    public void PauseMusic()
    {
        PauseAudio(musicAudioSource);
    }
    public void PlayAmbientClip(AudioClip clip)
    {
        PlayAudioClip(clip, ambientAudioSource);
    }
    public void PauseAmbient()
    {
        PauseAudio(ambientAudioSource);
    }

    private void PlayAudioClip(AudioClip clip, AudioSource audioSource)
    {
        if (audioSource)
        {
            audioSource.clip = clip;
            audioSource.Play();
        }
    }
    private void PauseAudio(AudioSource audioSource)
    {
        if (audioSource)
        {
            audioSource.Pause();
        }
    }
}
