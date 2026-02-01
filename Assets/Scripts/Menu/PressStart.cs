using UnityEngine;

public class PressStart : MonoBehaviour
{
    [Header("Pulse Settings")]
    [SerializeField] private float pulseSpeed = 1.5f;
    [SerializeField] private float pulseAmount = 0.05f;

    private Vector3 originalScale;

    void Awake()
    {
        MouseController.instance.ShowMouse();
        originalScale = transform.localScale;
    }

    void Update()
    {
        float pulse = Mathf.Sin(Time.unscaledTime * pulseSpeed) * pulseAmount;
        transform.localScale = originalScale + Vector3.one * pulse;
    }
}
