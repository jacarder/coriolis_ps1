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
    [SerializeField] TMP_Text interactionText;
    [SerializeField] TMP_Text questText;
    [SerializeField] GameObject dialoguePanel;
    [SerializeField] GameObject dice;
    [SerializeField] GameObject diceResultsContainer;
    [SerializeField] Camera dicCamera;

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