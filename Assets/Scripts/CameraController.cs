using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static CameraController instance;
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

    [Header("NPC Interaction Settings")]
    public float npcZoomFOV = 40f; // Zoomed in FOV (default is usually 60)
    public float npcTrackingSpeed = 5f; // How fast camera tracks NPC

    private bool isOrbiting = true;
    private bool isNPCInteraction = false;
    private float inactivityTimer = 0f;
    private float orbitAngle = 0f;
    private float transitionTimer = 0f;
    private bool isTransitioning = false;

    private Vector3 transitionStartPos;
    private Quaternion transitionStartRot;
    private Transform currentNPC;
    private float defaultFOV;

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

        // Store default FOV
        defaultFOV = firstPersonCamera.fieldOfView;
    }

    void Awake()
    {
        instance = this;
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
        else if (isNPCInteraction && currentNPC != null)
        {
            // Track NPC with first-person camera
            UpdateNPCTracking();
        }
        else if (isOrbiting)
        {
            UpdateOrbitCamera();
        }
    }

    void CheckForInput()
    {
        // Don't switch cameras during NPC interaction
        if (isNPCInteraction) return;

        // Check for keyboard input only (ignore mouse movement during potential NPC setup)
        bool hasKeyboardInput = Input.anyKeyDown;

        // Only check mouse if not transitioning
        bool hasMouseInput = !isTransitioning && (Mathf.Abs(Input.GetAxis("Mouse X")) > 0.1f || Mathf.Abs(Input.GetAxis("Mouse Y")) > 0.1f);

        if (hasKeyboardInput || hasMouseInput)
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
        // Don't return to orbit during NPC interaction
        if (!isOrbiting && !isTransitioning && !isNPCInteraction)
        {
            inactivityTimer += Time.deltaTime;

            if (inactivityTimer >= inactivityTime)
            {
                StartTransitionToOrbit();
            }
        }
        else if (isNPCInteraction)
        {
            // Reset timer during NPC interaction so it doesn't trigger immediately after
            inactivityTimer = 0f;
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

    void UpdateNPCTracking()
    {
        if (currentNPC == null || !firstPersonCamera.enabled) return;

        // Calculate direction to NPC
        Vector3 directionToNPC = currentNPC.position - firstPersonCamera.transform.position;
        Quaternion targetRotation = Quaternion.LookRotation(directionToNPC);

        // Smoothly rotate camera to look at NPC (X and Y axis)
        firstPersonCamera.transform.rotation = Quaternion.Slerp(
            firstPersonCamera.transform.rotation,
            targetRotation,
            Time.deltaTime * npcTrackingSpeed
        );

        // Smoothly zoom FOV
        // firstPersonCamera.fieldOfView = Mathf.Lerp(
        //     firstPersonCamera.fieldOfView,
        //     npcZoomFOV,
        //     Time.deltaTime * npcTrackingSpeed
        // );
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

    // Public method for HUD Controller to call
    public void StartNPCInteraction(Transform npc)
    {
        if (npc == null) return;

        currentNPC = npc;
        isNPCInteraction = true;
        inactivityTimer = 0f;

        // Force complete any ongoing transition
        if (isTransitioning)
        {
            isTransitioning = false;
        }

        // Make sure we're in first-person mode immediately
        if (!firstPersonCamera.enabled)
        {
            // Switch directly to first-person without transition
            mainCamera.enabled = false;
            firstPersonCamera.enabled = true;
            isOrbiting = false;
        }
    }

    // Public method to end NPC interaction
    public void EndNPCInteraction(bool returnToOrbit = true)
    {
        currentNPC = null;
        isNPCInteraction = false;

        // Reset FOV to default
        if (firstPersonCamera != null)
        {
            firstPersonCamera.fieldOfView = defaultFOV;
        }

        // if (returnToOrbit)
        // {
        //     StartTransitionToOrbit();
        // }
        // Otherwise stay in first-person with normal FOV
    }
}