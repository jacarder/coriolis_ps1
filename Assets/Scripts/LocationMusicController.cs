using UnityEngine;

public class LocationMusicController : MonoBehaviour
{

    public AudioClip musicClip;
    public AudioClip ambientClip;
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
        if (SoundController.instance)
        {

            if (isOnTarget && !isPlaying)
            {
                SoundController.instance.PlayMusicClip(musicClip);
                SoundController.instance.PlayAmbientClip(ambientClip);
                isPlaying = true;
                return;
            }

            if (!isOnTarget && isPlaying)
            {
                SoundController.instance.PauseMusic();
                SoundController.instance.PauseAmbient();
                isPlaying = false;
                return;
            }
        }

    }
}
