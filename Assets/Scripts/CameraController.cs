using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Camera References")]
    public Camera mainCamera; // The orbiting third-person camera
    public Camera firstPersonCamera; // The player's FPV camera

    [Header("Player Reference")]
    public Transform player;

    [Header("Orbit Settings")]
    public float orbitDistance = 5f;
    public float orbitHeight = 2f;
    public float orbitSpeed = 20f;

    [Header("Transition Settings")]
    public float inactivityTime = 3f;
    public float transitionDuration = 1f;

    private bool isOrbiting = true;
    private float inactivityTimer = 0f;
    private float orbitAngle = 0f;
    private float transitionTimer = 0f;
    private bool isTransitioning = false;

    private Vector3 transitionStartPos;
    private Quaternion transitionStartRot;

    void Start()
    {
        if (mainCamera == null)
        {
            Debug.LogError("Main Camera not assigned!");
            return;
        }

        if (firstPersonCamera == null)
        {
            Debug.LogError("First Person Camera not assigned!");
            return;
        }

        // Start with main camera active
        mainCamera.enabled = true;
        firstPersonCamera.enabled = false;
        orbitAngle = 0f;
    }

    void Update()
    {
        CheckForInput();
        UpdateInactivityTimer();
    }

    void LateUpdate()
    {
        if (mainCamera == null || player == null) return;

        if (isTransitioning)
        {
            UpdateCameraTransition();
        }
        else if (isOrbiting)
        {
            UpdateOrbitCamera();
        }
    }

    void CheckForInput()
    {
        // Check for any mouse or keyboard input
        if (Input.anyKey || Input.GetAxis("Mouse X") != 0 || Input.GetAxis("Mouse Y") != 0)
        {
            if (isOrbiting && !isTransitioning)
            {
                StartTransitionToFPV();
            }
            inactivityTimer = 0f;
        }
    }

    void UpdateInactivityTimer()
    {
        if (!isOrbiting && !isTransitioning)
        {
            inactivityTimer += Time.deltaTime;

            if (inactivityTimer >= inactivityTime)
            {
                StartTransitionToOrbit();
            }
        }
    }

    void UpdateCameraTransition()
    {
        transitionTimer += Time.deltaTime;
        float t = Mathf.Clamp01(transitionTimer / transitionDuration);

        // Smooth easing
        t = Mathf.SmoothStep(0f, 1f, t);

        if (isOrbiting)
        {
            // Transitioning from FPV back to orbit
            Vector3 targetPos = CalculateOrbitPosition();
            Quaternion targetRot = Quaternion.LookRotation(player.position - targetPos);

            mainCamera.transform.position = Vector3.Lerp(transitionStartPos, targetPos, t);
            mainCamera.transform.rotation = Quaternion.Slerp(transitionStartRot, targetRot, t);

            if (t >= 1f)
            {
                isTransitioning = false;
                firstPersonCamera.enabled = false;
                mainCamera.enabled = true;
            }
        }
        else
        {
            // Transitioning from orbit to FPV
            mainCamera.transform.position = Vector3.Lerp(transitionStartPos, firstPersonCamera.transform.position, t);
            mainCamera.transform.rotation = Quaternion.Slerp(transitionStartRot, firstPersonCamera.transform.rotation, t);

            if (t >= 1f)
            {
                isTransitioning = false;
                mainCamera.enabled = false;
                firstPersonCamera.enabled = true;
            }
        }
    }

    void StartTransitionToFPV()
    {
        isOrbiting = false;
        isTransitioning = true;
        transitionTimer = 0f;
        inactivityTimer = 0f;

        transitionStartPos = mainCamera.transform.position;
        transitionStartRot = mainCamera.transform.rotation;

        // Keep main camera active during transition
        mainCamera.enabled = true;
        firstPersonCamera.enabled = false;
    }

    void StartTransitionToOrbit()
    {
        isOrbiting = true;
        isTransitioning = true;
        transitionTimer = 0f;
        inactivityTimer = 0f;

        transitionStartPos = firstPersonCamera.transform.position;
        transitionStartRot = firstPersonCamera.transform.rotation;

        // Set orbit angle to start from behind the player
        Vector3 playerForward = player.forward;
        orbitAngle = Mathf.Atan2(-playerForward.z, -playerForward.x) * Mathf.Rad2Deg;

        // Switch to main camera for transition
        mainCamera.enabled = true;
        firstPersonCamera.enabled = false;
        mainCamera.transform.position = transitionStartPos;
        mainCamera.transform.rotation = transitionStartRot;
    }

    void UpdateOrbitCamera()
    {
        // Rotate around player
        orbitAngle += orbitSpeed * Time.deltaTime;
        if (orbitAngle >= 360f) orbitAngle -= 360f;

        Vector3 targetPos = CalculateOrbitPosition();
        mainCamera.transform.position = targetPos;
        mainCamera.transform.LookAt(player.position);
    }

    Vector3 CalculateOrbitPosition()
    {
        float rad = orbitAngle * Mathf.Deg2Rad;
        float x = player.position.x + Mathf.Cos(rad) * orbitDistance;
        float z = player.position.z + Mathf.Sin(rad) * orbitDistance;
        float y = player.position.y + orbitHeight;

        return new Vector3(x, y, z);
    }
}