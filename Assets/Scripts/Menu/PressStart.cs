using TMPro;
using UnityEngine;

public class PressStart : MonoBehaviour
{
    [Header("Scale Pulse")]
    public float pulseSpeed = 1f;
    public float pulseAmount = 0.05f;

    [Header("Alpha Pulse (Optional)")]
    public bool pulseAlpha = false;
    public float minAlpha = 0.7f;
    public float maxAlpha = 1f;

    private Vector3 startScale;
    private TextMeshProUGUI tmpText;

    void Awake()
    {
        startScale = transform.localScale;
        tmpText = GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        float pulse = Mathf.Sin(Time.time * pulseSpeed) * pulseAmount;

        // Scale pulse
        transform.localScale = startScale + Vector3.one * pulse;

        // Alpha pulse (optional)
        if (pulseAlpha && tmpText != null)
        {
            float alpha = Mathf.Lerp(minAlpha, maxAlpha, (Mathf.Sin(Time.time * pulseSpeed) + 1f) / 2f);
            Color c = tmpText.color;
            c.a = alpha;
            tmpText.color = c;
        }

    }
}
