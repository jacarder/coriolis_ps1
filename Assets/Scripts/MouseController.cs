using UnityEngine;

public class MouseController : MonoBehaviour
{
    public static MouseController instance;
    public Texture2D mainMouseTexture;
    void Awake()
    {
        instance = this;
        SetMouseTexture();
    }

    public void ShowMouse()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

    }
    public void HideMouse()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    private void SetMouseTexture()
    {
        Cursor.SetCursor(mainMouseTexture, Vector2.zero, CursorMode.Auto);
    }
}
