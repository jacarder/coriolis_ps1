using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HUDController : MonoBehaviour
{
    public static HUDController instance;
    private void Awake()
    {
        instance = this;
        DisableDice();
        DisableDialog();
    }
    public TMP_Text interactionText;
    public TMP_Text questText;
    public GameObject dialoguePanel;
    public GameObject dice;
    public GameObject diceResultsContainer;
    public Camera dicCamera;

    public void EnableInteractionText(string text)
    {
        interactionText.text = text + " (F)";
        interactionText.gameObject.SetActive(true);
    }
    public void DisableInteractionText()
    {
        if (interactionText)
        {
            interactionText.gameObject.SetActive(false);
        }
    }

    public void EnableDialog()
    {
        dialoguePanel.SetActive(true);
    }
    public void DisableDialog()
    {
        dialoguePanel.SetActive(false);
    }

    public void EnableDice()
    {
        dice.SetActive(true);
        diceResultsContainer.SetActive(true);
    }

    public void DisableDice()
    {
        dice.SetActive(false);
        diceResultsContainer.SetActive(false);
    }
}