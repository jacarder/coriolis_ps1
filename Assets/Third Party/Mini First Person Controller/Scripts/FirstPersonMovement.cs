using System.Collections.Generic;
using UnityEngine;

public class FirstPersonMovement : MonoBehaviour
{
    public float speed = 5;
    public static FirstPersonMovement instance;

    [Header("Running")]
    public bool canRun = true;
    public bool IsRunning { get; private set; }
    public float runSpeed = 9;
    public KeyCode runningKey = KeyCode.LeftShift;
    public VectorValueSO startingPosition;
    public QuaternionValueSO startingRotation;
    private bool allowMove = true;

    Rigidbody rigidbody;
    /// <summary> Functions to override movement speed. Will use the last added override. </summary>
    public List<System.Func<float>> speedOverrides = new List<System.Func<float>>();



    void Awake()
    {
        instance = this;
        // Get the rigidbody on this.
        rigidbody = GetComponent<Rigidbody>();
        UpdatePlayerPosition();
        UpdatePlayerRotation();
    }

    void FixedUpdate()
    {
        if (allowMove)
        {
            // Update IsRunning from input.
            IsRunning = canRun && Input.GetKey(runningKey);

            // Get targetMovingSpeed.
            float targetMovingSpeed = IsRunning ? runSpeed : speed;
            if (speedOverrides.Count > 0)
            {
                targetMovingSpeed = speedOverrides[speedOverrides.Count - 1]();
            }

            // Get targetVelocity from input.
            Vector2 targetVelocity = new Vector2(Input.GetAxis("Horizontal") * targetMovingSpeed, Input.GetAxis("Vertical") * targetMovingSpeed);

            // Apply movement.
            rigidbody.linearVelocity = transform.rotation * new Vector3(targetVelocity.x, rigidbody.linearVelocity.y, targetVelocity.y);
        }
    }

    public void StopMovement()
    {
        allowMove = false;
    }

    public void StartMovement()
    {
        allowMove = true;
    }

    public void UpdatePlayerPosition()
    {
        transform.position = startingPosition.initialValue;
    }
    public void UpdatePlayerRotation()
    {
        transform.rotation = startingRotation.initialValue;

        if (FirstPersonLook.instance != null)
        {
            FirstPersonLook.instance.SyncRotationFromTransform();
            FirstPersonLook.instance.LevelView();
        }
    }
}