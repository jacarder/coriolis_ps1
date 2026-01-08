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

    public void Interact()
    {
        onInteraction.Invoke();
    }
}
