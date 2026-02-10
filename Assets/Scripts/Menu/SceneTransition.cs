using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SceneTransition : MonoBehaviour, IPointerDownHandler
{
    public Image fadeImage;
    public float fadeDuration = 1f;
    public string sceneToLoad;

    private bool isTransitioning = false;

    void Start()
    {
        fadeImage.enabled = false;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (!isTransitioning)
            ScreenFadeController.Instance.FadeToScene(sceneToLoad);
    }
}
