using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    public static MusicPlayer instance;
    public AudioSource audioSource;
    void Awake()
    {
        instance = this;
    }
    public void PlayMusic(AudioClip clip)
    {
        audioSource.clip = clip;
        audioSource.Play();
    }
    public void PauseMusic()
    {
        audioSource.Pause();
    }
}
