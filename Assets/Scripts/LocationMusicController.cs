using UnityEngine;

public class LocationMusicController : MonoBehaviour
{

    public AudioSource audioSource;
    public AudioClip clip;
    private bool isOnTarget = false;
    private bool isPlaying = false;

    void OnTriggerEnter(Collider collider)
    {
        // Check if the object we are colliding with has the target tag
        if (collider.CompareTag("Player"))
        {
            isOnTarget = true;
        }
    }

    void OnTriggerExit(Collider collider)
    {
        // Reset when leaving the object
        if (collider.CompareTag("Player"))
        {
            isOnTarget = false;
        }
    }

    void Update()
    {
        if (isOnTarget && !isPlaying)
        {
            PlayMusic(clip);
            isPlaying = true;
            return;
        }

        if (!isOnTarget && isPlaying)
        {
            PauseMusic();
            isPlaying = false;
            return;
        }

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
