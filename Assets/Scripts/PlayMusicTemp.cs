using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class PlayMusicOnLoad : MonoBehaviour
{
    public AudioClip musicClip;  // Assign your music clip in the inspector
    [Range(0f, 1f)]
    public float volume = 1f;    // Music volume

    private AudioSource audioSource;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();

        // Setup AudioSource
        audioSource.clip = musicClip;
        audioSource.volume = volume;
        audioSource.loop = true; // Loop the music
        audioSource.playOnAwake = false;

        // Play music immediately
        if (musicClip != null)
            audioSource.Play();
        else
            Debug.LogWarning("No music clip assigned!");
    }
}
