using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ScreenFadeController : MonoBehaviour
{
    public static ScreenFadeController Instance;

    public Image fadeImage;
    private float fadeDuration = 1f;

    void OnEnable()
    {
        // Subscribe to the sceneLoaded event
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        // Unsubscribe to prevent memory leaks
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        FirstPersonMovement.instance.UpdatePlayerPosition();
        StartCoroutine(FadeIn());
    }

    void Awake()
    {
        Instance = this;
    }

    public void FadeToScene(string sceneName)
    {
        StartCoroutine(FadeOutAndLoad(sceneName));
    }

    IEnumerator FadeOutAndLoad(string sceneName)
    {
        // Freeze game
        Time.timeScale = 0f;
        SoundController.instance?.StopAllSound();
        yield return StartCoroutine(Fade(0f, 1f));
        HUDController.instance?.DisableInteractionText();
        SceneManager.LoadScene(sceneName);
    }

    IEnumerator FadeIn()
    {
        yield return StartCoroutine(Fade(1f, 0f));
        //  Unfreeze
        Time.timeScale = 1f;
    }

    IEnumerator Fade(float startAlpha, float endAlpha)
    {
        fadeImage.enabled = true;
        float elapsed = 0f;
        Color color = fadeImage.color;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.unscaledDeltaTime; ;
            float alpha = Mathf.Lerp(startAlpha, endAlpha, elapsed / fadeDuration);
            fadeImage.color = new Color(color.r, color.g, color.b, alpha);
            yield return null;
        }

        fadeImage.color = new Color(color.r, color.g, color.b, endAlpha);
    }
}
