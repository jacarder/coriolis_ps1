//  AI Code
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Camera))]
public class CameraToRawImage : MonoBehaviour
{
    [Header("Render Texture Settings")]
    public int width = 1024;
    public int height = 1024;
    public int depth = 24;

    [Header("UI Target")]
    public RawImage rawImage;

    private Camera cam;
    private RenderTexture rt;

    void Awake()
    {
        cam = GetComponent<Camera>();

        // Create unique RenderTexture
        rt = new RenderTexture(width, height, depth, RenderTextureFormat.ARGB32);
        rt.name = $"{gameObject.name}_RT";
        rt.Create();

        // Camera renders INTO the texture
        cam.targetTexture = rt;

        // UI reads FROM the texture
        if (rawImage != null)
            rawImage.texture = rt;
    }

    void OnDestroy()
    {
        if (cam != null)
            cam.targetTexture = null;

        if (rawImage != null)
            rawImage.texture = null;

        if (rt != null)
            rt.Release();
    }
}
