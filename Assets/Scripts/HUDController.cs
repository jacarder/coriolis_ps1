using TMPro;
using UnityEngine;

public class HUDController : MonoBehaviour
{
    public static HUDController instance;
    private void Awake()
    {
        instance = this;
        DisableDialog();
    }
    [SerializeField] TMP_Text interactionText;
    [SerializeField] GameObject dialoguePanel;
    public void EnableInteractionText(string text)
    {
        interactionText.text = text + " (F)";
        interactionText.gameObject.SetActive(true);
    }
    public void DisableInteractionText()
    {
        interactionText.gameObject.SetActive(false);
    }

    public void EnableDialog()
    {
        dialoguePanel.SetActive(true);
    }
    public void DisableDialog()
    {
        dialoguePanel.SetActive(false);
    }
}