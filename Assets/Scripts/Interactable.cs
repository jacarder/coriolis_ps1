using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Interactable : MonoBehaviour
{
    FloatInteractionText interactionText;
    public string message;
    public UnityEvent onInteraction;

    void Start()
    {
        // outline = GetComponent<Outline>;
        // DisableOutline();
    }

    public void EnableFloatInteractionText(string text)
    {
        interactionText.EnableInteractionText(text);
    }

    public void DisableFloatInteractionText()
    {
        interactionText.DisableInteractionText();
    }

    public void Interact()
    {
        //  TODO bring up
        onInteraction.Invoke();
    }
}
