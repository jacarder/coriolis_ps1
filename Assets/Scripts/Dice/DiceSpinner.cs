using UnityEngine;
using System.Collections;

public class DiceSpinner : MonoBehaviour
{
    [Header("Spin Timing")]
    public float warmUpDuration = 0.5f;
    public float spinDuration = 1.5f;
    public float snapDuration = 0.4f;

    [Header("Spin Speed")]
    public float maxSpinSpeed = 720f;

    private bool isSpinning;

    // Die face rotations (face UP)
    private readonly Quaternion[] faceRotations =
    {
        Quaternion.Euler(0, 0, 0),     // 1
        Quaternion.Euler(0, 0, 180),   // 6
        Quaternion.Euler(0, 0, -90),   // 2
        Quaternion.Euler(0, 0, 90),    // 5
        Quaternion.Euler(-90, 0, 0),   // 3
        Quaternion.Euler(90, 0, 0)     // 4
    };

    public void RollToFace(int face, System.Action onFinished = null)
    {
        if (isSpinning || face < 1 || face > 6)
            return;

        StartCoroutine(SpinAndStop(face, onFinished));
    }

    private IEnumerator SpinAndStop(int face, System.Action onFinished)
    {
        isSpinning = true;

        Vector3 spinAxis = Random.onUnitSphere;

        // 🔥 Warm-up phase (ease in)
        float t = 0f;
        while (t < warmUpDuration)
        {
            t += Time.deltaTime;
            float speed = Mathf.Lerp(0f, maxSpinSpeed, t / warmUpDuration);

            transform.Rotate(spinAxis * speed * Time.deltaTime, Space.World);
            yield return null;
        }

        // 🌪️ Full spin phase
        t = 0f;
        while (t < spinDuration)
        {
            t += Time.deltaTime;

            transform.Rotate(
                Random.onUnitSphere * maxSpinSpeed * Time.deltaTime,
                Space.World
            );

            yield return null;
        }

        // 🎯 Snap to final face
        Quaternion targetRotation = faceRotations[face - 1];
        t = 0f;

        Quaternion startRotation = transform.rotation;

        while (t < 1f)
        {
            t += Time.deltaTime / snapDuration;
            transform.rotation = Quaternion.Slerp(startRotation, targetRotation, t);
            yield return null;
        }

        transform.rotation = targetRotation;
        isSpinning = false;
        onFinished?.Invoke();
    }
}
