using System.Collections;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DoorTransition : MonoBehaviour
{
    [Header("Prefab Root (THIS object)")]
    public Transform doorRoot;

    [Header("Movement")]
    public Vector3 slideOffset = new Vector3(0f, 0f, 2f);
    public float slideDuration = 1f;

    private float fadeDuration = 1f;

    public Image fadeImage;

    private string sceneToLoad;

    Vector3 startPos;
    Vector3 endPos;

    void Awake()
    {
        // If not assigned, assume script is on the root
        if (doorRoot == null)
            doorRoot = transform;

        fadeImage = GameObject.Find("FadeImage")?.GetComponent<Image>();
        startPos = doorRoot.position;
        endPos = startPos + slideOffset;
    }

    public void ActivateDoor(string sceneToLoad)
    {
        this.sceneToLoad = sceneToLoad;
        StartCoroutine(DoorSequence(() =>
        {
            ScreenFadeController.Instance.FadeToScene(sceneToLoad);
        }));
    }

    IEnumerator DoorSequence(System.Action callback)
    {
        float t = 0f;
        while (t < slideDuration)
        {
            t += Time.deltaTime;
            doorRoot.position = Vector3.Lerp(startPos, endPos, t / slideDuration);
            yield return null;
        }

        doorRoot.position = endPos;
        callback();
    }
}
