using UnityEngine;

public class MenuController : MonoBehaviour
{
    public static MenuController instance;
    [HideInInspector]
    public bool isShowing;
    void Awake()
    {
        gameObject.SetActive(false);
        instance = this;
    }
    public void ShowMenu()
    {
        isShowing = true;
        Time.timeScale = 0f;
        MouseController.instance.ShowMouse();
        FirstPersonLook.instance.StopMovement();
        gameObject.SetActive(true);

    }

    public void HideMenu()
    {
        isShowing = false;
        Time.timeScale = 1f;
        MouseController.instance.HideMouse();
        FirstPersonLook.instance.StartMovement();
        gameObject.SetActive(false);
    }
}
