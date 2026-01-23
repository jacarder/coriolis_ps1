using System.Collections;
using TMPro;
using UnityEngine;

public class DiceResultNotification : MonoBehaviour
{

    public float revealSpeed = 2.5f;
    public float fadeOutSpeed = 2f;
    public float holdTime = 5f;

    public float windFrequency = 2.5f;
    public float windAmplitude = 0.6f;

    public float alphaStep = 0.5f; // PS1-style alpha stepping
    public float jitterAmount = 0.25f;
    private TMP_TextInfo textInfo;
    private TMP_Text notification;
    public AudioSource audioSource;
    public AudioClip limitedSuccessClip;
    public AudioClip criticalClip;
    public AudioClip failureClip;
    private void Awake()
    {
        notification = GetComponent<TMP_Text>();
        textInfo = notification.textInfo;
        notification.text = "";
    }
    private void OnEnable()
    {
        GameEventsManager.instance.diceEvents.onDiceRollResponse += HandleDiceResult;
    }
    private void OnDisable()
    {
        GameEventsManager.instance.diceEvents.onDiceRollResponse += HandleDiceResult;
    }
    private void HandleDiceResult(DiceResult diceResult)
    {
        switch (diceResult.result)
        {
            case DiceSuccessState.CRITICAL_SUCCESS:
                audioSource.PlayOneShot(criticalClip);
                notification.text = "Critical Success!";
                break;
            case DiceSuccessState.LIMITED_SUCCESS:
                audioSource.PlayOneShot(limitedSuccessClip);
                notification.text = "Limited Success!";
                break;
            case DiceSuccessState.FAILURE:
                audioSource.PlayOneShot(failureClip);
                notification.text = "Failure!";
                break;
        }
        notification.ForceMeshUpdate();
        SetAllAlpha(0);
        StartCoroutine(WindLifecycle());
    }

    IEnumerator WindLifecycle()
    {
        yield return WindFade(true);   // Fade IN
        yield return new WaitForSeconds(holdTime);
        yield return WindFade(false);  // Fade OUT
    }

    IEnumerator WindFade(bool fadeIn)
    {
        float time = 0f;

        while (true)
        {
            notification.ForceMeshUpdate();
            textInfo = notification.textInfo;

            bool finished = true;

            for (int i = 0; i < textInfo.characterCount; i++)
            {
                var charInfo = textInfo.characterInfo[i];
                if (!charInfo.isVisible) continue;

                int matIndex = charInfo.materialReferenceIndex;
                int vertIndex = charInfo.vertexIndex;

                Color32[] colors = textInfo.meshInfo[matIndex].colors32;
                Vector3[] verts = textInfo.meshInfo[matIndex].vertices;

                float wind =
                    Mathf.Sin((fadeIn ? time : -time) * windFrequency + i * 0.35f);

                float target =
                    wind * windAmplitude + (fadeIn ? time : 1f - time);

                target = Mathf.Clamp01(target);

                // PS1 stepped alpha
                target = Mathf.Floor(target / alphaStep) * alphaStep;

                byte alpha = (byte)(target * 255);

                if ((fadeIn && alpha < 250) || (!fadeIn && alpha > 5))
                    finished = false;

                for (int v = 0; v < 4; v++)
                    colors[vertIndex + v].a = alpha;

                // Sand jitter
                Vector3 jitter = new Vector3(
                    Random.Range(-jitterAmount, jitterAmount),
                    Random.Range(-jitterAmount, jitterAmount),
                    0f
                ) * (fadeIn ? (1f - target) : target);

                for (int v = 0; v < 4; v++)
                    verts[vertIndex + v] += jitter;
            }

            for (int i = 0; i < textInfo.meshInfo.Length; i++)
            {
                var meshInfo = textInfo.meshInfo[i];
                meshInfo.mesh.vertices = meshInfo.vertices;
                meshInfo.mesh.colors32 = meshInfo.colors32;
                notification.UpdateGeometry(meshInfo.mesh, i);
            }

            if (finished)
                yield break;

            time += Time.deltaTime * (fadeIn ? revealSpeed : fadeOutSpeed);
            yield return null;
        }
    }

    void SetAllAlpha(byte a)
    {
        notification.ForceMeshUpdate();
        var ti = notification.textInfo;

        for (int i = 0; i < ti.meshInfo.Length; i++)
        {
            var colors = ti.meshInfo[i].colors32;
            for (int c = 0; c < colors.Length; c++)
                colors[c].a = a;

            ti.meshInfo[i].mesh.colors32 = colors;
            notification.UpdateGeometry(ti.meshInfo[i].mesh, i);
        }
    }
}

