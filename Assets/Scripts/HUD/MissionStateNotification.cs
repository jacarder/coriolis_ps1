using System.Collections;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(TextMeshProUGUI))]
[RequireComponent(typeof(AudioSource))]
public class MissionStateNotification : MonoBehaviour
{
    [Header("Mission Text")]
    public string missionStartedText = "MISSION START";
    public string missionSuccessText = "MISSION SUCCESS";
    public string missionFailedText = "MISSION FAILED";

    [Header("Timing Defaults")]
    public float defaultFadeInDuration = 1.2f;
    public float defaultHoldDuration = 2.0f;
    public float defaultFadeOutDuration = 1.2f;

    [Header("PS1 Fade Style")]
    [Range(2, 32)]
    public int alphaSteps = 8;          // Chunkiness
    [Range(5f, 30f)]
    public float fadeTickRate = 12f;    // Low "FPS"
    public bool jitterTiming = true;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip startSound;
    public AudioClip finishedSound;

    private TextMeshProUGUI tmp;
    private Coroutine stateRoutine;
    private QuestState questState;

    void Awake()
    {
        tmp = GetComponent<TextMeshProUGUI>();
        audioSource = GetComponent<AudioSource>();

        tmp.text = "";
        SetAlpha(0f);
    }

    private void OnEnable()
    {
        GameEventsManager.instance.questEvents.onQuestStateChange += HandleQuestStateChange;
    }
    private void OnDisable()
    {
        GameEventsManager.instance.questEvents.onQuestStateChange += HandleQuestStateChange;
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
                ShowMissionSuccess();
                break;
        }
    }

    private void ShowMissionStarted()
    {
        ShowText(missionStartedText,
            defaultFadeInDuration,
            defaultHoldDuration,
            defaultFadeOutDuration);
    }

    private void ShowMissionSuccess()
    {
        ShowText(missionSuccessText,
            defaultFadeInDuration,
            defaultHoldDuration,
            defaultFadeOutDuration);
    }

    private void ShowMissionFailed()
    {
        ShowText(missionFailedText,
            defaultFadeInDuration,
            defaultHoldDuration,
            defaultFadeOutDuration);
    }

    private void ShowText(
        string text,
        float fadeInDuration,
        float holdDuration,
        float fadeOutDuration
    )
    {
        if (stateRoutine != null)
            StopCoroutine(stateRoutine);

        tmp.text = text;
        tmp.ForceMeshUpdate();
        SetAlpha(0f);

        stateRoutine = StartCoroutine(StateSequence(
            fadeInDuration,
            holdDuration,
            fadeOutDuration));
    }

    private IEnumerator StateSequence(
        float fadeInDuration,
        float holdDuration,
        float fadeOutDuration
    )
    {
        yield return FadeText(0f, 1f, fadeInDuration, GetSoundToPlay());
        yield return new WaitForSeconds(holdDuration);
        yield return FadeText(1f, 0f, fadeOutDuration, null);
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

    // === PS1-STYLE FADE ===

    private IEnumerator FadeText(
        float startAlpha,
        float endAlpha,
        float duration,
        AudioClip sound
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

            float rawAlpha = Mathf.Lerp(startAlpha, endAlpha, t);
            float steppedAlpha = Mathf.Round(rawAlpha * alphaSteps) / alphaSteps;

            // Extra PS1 harsh cutoff
            if (steppedAlpha < 0.05f)
                steppedAlpha = 0f;

            SetAlpha(steppedAlpha);

            float jitter = jitterTiming ? Random.Range(-0.01f, 0.01f) : 0f;
            yield return new WaitForSeconds(Mathf.Max(0f, tickInterval + jitter));
        }

        SetAlpha(endAlpha);
    }

    // === VERTEX COLOR ALPHA ===

    private void SetAlpha(float alpha)
    {
        tmp.ForceMeshUpdate();
        var textInfo = tmp.textInfo;

        for (int i = 0; i < textInfo.characterCount; i++)
        {
            var charInfo = textInfo.characterInfo[i];
            if (!charInfo.isVisible)
                continue;

            int vertexIndex = charInfo.vertexIndex;
            int materialIndex = charInfo.materialReferenceIndex;

            var colors = textInfo.meshInfo[materialIndex].colors32;

            byte a = (byte)(alpha * 255);

            colors[vertexIndex + 0].a = a;
            colors[vertexIndex + 1].a = a;
            colors[vertexIndex + 2].a = a;
            colors[vertexIndex + 3].a = a;
        }

        tmp.UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32);
    }
}
