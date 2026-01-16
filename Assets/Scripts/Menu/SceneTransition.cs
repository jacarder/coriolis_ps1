using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;
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
            StartCoroutine(Transition());
    }

    IEnumerator Transition()
    {
        isTransitioning = true;

        // Freeze game
        Time.timeScale = 0f;
        MusicPlayer.instance.audioSource.Stop();

        // Fade Out
        yield return StartCoroutine(Fade(0f, 1f));

        // Load new scene
        Time.timeScale = 1f;
        SceneManager.LoadScene(sceneToLoad);
    }

    IEnumerator Fade(float startAlpha, float endAlpha)
    {
        fadeImage.enabled = true;
        float elapsed = 0f;
        Color color = fadeImage.color;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.unscaledDeltaTime;
            float alpha = Mathf.Lerp(startAlpha, endAlpha, elapsed / fadeDuration);
            fadeImage.color = new Color(color.r, color.g, color.b, alpha);
            yield return null;
        }

        fadeImage.color = new Color(color.r, color.g, color.b, endAlpha);
    }
}
