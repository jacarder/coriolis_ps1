using TMPro;
using UnityEngine;

public class FloatInteractionText : MonoBehaviour
{
    public static FloatInteractionText instance;
    private void Awake()
    {
        instance = this;
    }
    [SerializeField] TextMeshPro interactionText;
    public void EnableInteractionText(string text)
    {
        interactionText.transform.position = gameObject.transform.position;
        interactionText.text = text + " (F)";
        interactionText.gameObject.SetActive(true);
    }
    public void DisableInteractionText()
    {
        interactionText.gameObject.SetActive(false);
    }
}
