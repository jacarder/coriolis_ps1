using UnityEngine;

public class LocationMusicController : MonoBehaviour
{

    public AudioClip clip;
    private bool isOnTarget = false;
    private bool isPlaying = false;

    void OnCollisionStay(Collision collision)
    {
        // Check if the object we are colliding with has the target tag
        if (collision.gameObject.CompareTag("Player"))
        {
            isOnTarget = true;
        }
    }

    void OnCollisionExit(Collision collision)
    {
        // Reset when leaving the object
        if (collision.gameObject.CompareTag("Player"))
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
