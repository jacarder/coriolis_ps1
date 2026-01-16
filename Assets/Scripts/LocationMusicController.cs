using UnityEngine;

public class LocationMusicController : MonoBehaviour
{

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
            MusicPlayer.instance.PlayMusic(clip);
            isPlaying = true;
            return;
        }

        if (!isOnTarget && isPlaying)
        {
            MusicPlayer.instance.PauseMusic();
            isPlaying = false;
            return;
        }

    }
}
