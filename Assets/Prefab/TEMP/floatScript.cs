using UnityEngine;

public class RealisticHover : MonoBehaviour
{
    [Header("Vertical Float")]
    [Tooltip("Height of the floating motion")]
    public float floatAmplitude = 0.3f;

    [Tooltip("Speed of up and down motion")]
    public float floatSpeed = 1.5f;

    [Header("Gentle Rotation")]
    [Tooltip("Enable slow rotation")]
    public bool enableRotation = true;

    [Tooltip("Speed of Y-axis rotation")]
    public float rotationSpeed = 15f;

    [Header("Subtle Tilt")]
    [Tooltip("Enable subtle tilting motion")]
    public bool enableTilt = true;

    [Tooltip("Maximum tilt angle in degrees")]
    public float tiltAmplitude = 3f;

    [Tooltip("Speed of tilting motion")]
    public float tiltSpeed = 0.8f;

    [Header("Noise (Advanced)")]
    [Tooltip("Add Perlin noise for organic movement")]
    public bool useNoise = true;

    [Tooltip("Strength of noise effect")]
    public float noiseStrength = 0.05f;

    private Vector3 startPos;
    private Quaternion startRot;
    private float timeOffset;

    void Start()
    {
        startPos = transform.position;
        startRot = transform.rotation;

        // Random offset so multiple objects don't move in sync
        timeOffset = Random.Range(0f, 100f);
    }

    void Update()
    {
        float t = Time.time + timeOffset;

        // Smooth vertical floating using sine wave
        float yOffset = Mathf.Sin(t * floatSpeed) * floatAmplitude;

        // Add Perlin noise for organic variation
        if (useNoise)
        {
            float noiseX = Mathf.PerlinNoise(t * 0.5f, 0) - 0.5f;
            float noiseZ = Mathf.PerlinNoise(0, t * 0.5f) - 0.5f;

            Vector3 noiseOffset = new Vector3(
                noiseX * noiseStrength,
                0,
                noiseZ * noiseStrength
            );

            transform.position = startPos + new Vector3(0, yOffset, 0) + noiseOffset;
        }
        else
        {
            transform.position = startPos + new Vector3(0, yOffset, 0);
        }

        // Subtle tilting motion
        if (enableTilt)
        {
            float tiltX = Mathf.Sin(t * tiltSpeed) * tiltAmplitude;
            float tiltZ = Mathf.Cos(t * tiltSpeed * 0.7f) * tiltAmplitude;

            Quaternion tiltRotation = Quaternion.Euler(tiltX, 0, tiltZ);

            if (enableRotation)
            {
                Quaternion yRotation = Quaternion.Euler(0, t * rotationSpeed, 0);
                transform.rotation = startRot * yRotation * tiltRotation;
            }
            else
            {
                transform.rotation = startRot * tiltRotation;
            }
        }
        else if (enableRotation)
        {
            Quaternion yRotation = Quaternion.Euler(0, t * rotationSpeed, 0);
            transform.rotation = startRot * yRotation;
        }
    }
}