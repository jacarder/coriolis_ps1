using UnityEngine;

public class FirstPersonLook : MonoBehaviour
{
    public static FirstPersonLook instance;

    [SerializeField]
    Transform character;

    public float sensitivity = 2;
    public float smoothing = 1.5f;

    Vector2 velocity;
    Vector2 frameVelocity;

    private bool allowMove = true;

    void Awake()
    {
        instance = this;
    }

    void Reset()
    {
        character = GetComponentInParent<FirstPersonMovement>().transform;
    }

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;

        // initialize look rotation from movement rotation
        Vector3 characterEuler = character.rotation.eulerAngles;
        Vector3 cameraEuler = transform.localRotation.eulerAngles;

        // Unity uses 0–360, convert to -180–180
        velocity.x = characterEuler.y;
        velocity.y = cameraEuler.x > 180 ? cameraEuler.x - 360 : cameraEuler.x;
    }

    void Update()
    {
        if (!allowMove) return;

        Vector2 mouseDelta = new Vector2(
            Input.GetAxisRaw("Mouse X"),
            Input.GetAxisRaw("Mouse Y")
        );

        Vector2 rawFrameVelocity = mouseDelta * sensitivity;
        frameVelocity = Vector2.Lerp(frameVelocity, rawFrameVelocity, 1f / smoothing);

        velocity += frameVelocity;
        velocity.y = Mathf.Clamp(velocity.y, -90f, 90f);

        // Apply rotations RELATIVE to initialized values
        transform.localRotation = Quaternion.Euler(-velocity.y, 0f, 0f);
        character.rotation = Quaternion.Euler(0f, velocity.x, 0f);
    }

    public void StopMovement()
    {
        allowMove = false;
    }

    public void StartMovement()
    {
        allowMove = true;
    }

    public void SyncRotationFromTransform()
    {
        Vector3 characterEuler = character.rotation.eulerAngles;
        Vector3 cameraEuler = transform.localRotation.eulerAngles;

        velocity.x = characterEuler.y;
        velocity.y = cameraEuler.x > 180f ? cameraEuler.x - 360f : cameraEuler.x;

        frameVelocity = Vector2.zero;
    }
    public void LevelView()
    {
        // Reset pitch
        velocity.y = 0f;
        frameVelocity = Vector2.zero;

        // Apply rotations immediately
        transform.localRotation = Quaternion.identity;
        character.rotation = Quaternion.Euler(0f, velocity.x, 0f);
    }
}
