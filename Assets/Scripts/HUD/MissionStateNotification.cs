using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class MissionStateImage : MonoBehaviour
{
    [Header("State Materials")]
    public Material missionStartedMaterial;
    public Material missionCompleteMaterial;
    public Material missionFailedMaterial;

    [Header("Fade Settings")]
    public float defaultFadeInDuration = 1.5f;
    public float defaultHoldDuration = 2.0f;
    public float defaultFadeOutDuration = 1.5f;

    [Header("PS1 Fade Style")]
    [Range(2, 32)]
    public int alphaSteps = 8;     // Lower = chunkier
    [Range(5f, 30f)]
    public float fadeTickRate = 12f; // "FPS" of the fade
    public bool jitterTiming = true;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip startSound;
    public AudioClip finishedSound;

    private Image image;
    private Coroutine stateRoutine;
    private Material runtimeMaterial;
    private QuestState questState;

    private void OnEnable()
    {
        GameEventsManager.instance.questEvents.onQuestStateChange += HandleQuestStateChange;
    }
    private void OnDisable()
    {
        GameEventsManager.instance.questEvents.onQuestStateChange += HandleQuestStateChange;
    }

    private void Awake()
    {
        image = GetComponent<Image>();
        gameObject.SetActive(false);
    }

    private void HandleQuestStateChange(Quest quest)
    {
        questState = quest.state;
        switch (quest.state)
        {
            case QuestState.IN_PROGRESS:
                ShowMissionStarted();
                break;
            case QuestState.FINISHED:
                ShowMissionComplete();
                break;
        }
    }

    public void ShowMissionStarted()
    {
        ShowState(
            missionStartedMaterial,
            defaultFadeInDuration,
            defaultHoldDuration,
            defaultFadeOutDuration
        );
    }

    public void ShowMissionComplete()
    {
        ShowState(
            missionCompleteMaterial,
            defaultFadeInDuration,
            defaultHoldDuration,
            defaultFadeOutDuration
        );
    }

    public void ShowMissionFailed()
    {
        ShowState(
            missionFailedMaterial,
            defaultFadeInDuration,
            defaultHoldDuration,
            defaultFadeOutDuration
        );
    }


    public void ShowState(
        Material sourceMaterial,
        float fadeInDuration,
        float holdDuration,
        float fadeOutDuration
    )
    {
        gameObject.SetActive(true);
        if (stateRoutine != null)
            StopCoroutine(stateRoutine);

        // Destroy previous runtime material
        if (runtimeMaterial != null)
            Destroy(runtimeMaterial);

        // Create instance
        runtimeMaterial = new Material(sourceMaterial);
        image.material = runtimeMaterial;

        SetMaterialAlpha(0f);

        stateRoutine = StartCoroutine(StateSequence(
            fadeInDuration,
            holdDuration,
            fadeOutDuration
        ));
    }

    private IEnumerator StateSequence(
        float fadeInDuration,
        float holdDuration,
        float fadeOutDuration
    )
    {
        yield return FadeMaterial(0f, 1f, fadeInDuration, GetSoundToPlay());
        yield return new WaitForSeconds(holdDuration);
        yield return FadeMaterial(1f, 0f, fadeOutDuration);
        gameObject.SetActive(false);
    }

    private IEnumerator FadeMaterial(
        float startAlpha,
        float endAlpha,
        float duration,
        AudioClip sound = null
    )
    {
        if (sound != null)
            audioSource.PlayOneShot(sound);

        float elapsed = 0f;
        float tickInterval = 1f / fadeTickRate;

        while (elapsed < duration)
        {
            elapsed += tickInterval;

            float t = Mathf.Clamp01(elapsed / duration);

            // Quantized alpha (PS1 step look)
            float rawAlpha = Mathf.Lerp(startAlpha, endAlpha, t);
            float steppedAlpha = Mathf.Round(rawAlpha * alphaSteps) / alphaSteps;

            SetMaterialAlpha(steppedAlpha);

            // Optional jitter for extra crunch
            float jitter = jitterTiming ? Random.Range(-0.01f, 0.01f) : 0f;
            yield return new WaitForSeconds(Mathf.Max(0f, tickInterval + jitter));
        }

        SetMaterialAlpha(endAlpha);
    }

    private void SetMaterialAlpha(float alpha)
    {
        Color c = runtimeMaterial.color;
        c.a = alpha;
        runtimeMaterial.color = c;
    }

    private AudioClip GetSoundToPlay()
    {
        AudioClip soundToPlay = null;
        switch (questState)
        {
            case QuestState.IN_PROGRESS:
                soundToPlay = startSound;
                break;
            case QuestState.FINISHED:
                soundToPlay = finishedSound;
                break;
        }
        return soundToPlay;
    }

    void OnDestroy()
    {
        if (runtimeMaterial != null)
            Destroy(runtimeMaterial);
    }
}
