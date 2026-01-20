using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance { get; private set; }

    // UI references
    public GameObject DialogueParent; // Main container for dialogue UI
    public TextMeshProUGUI DialogTitleText, DialogBodyText; // Text components for title and body
    public GameObject responseButtonPrefab; // Prefab for generating response buttons
    public Transform responseButtonContainer; // Container to hold response buttons
    private DialogueNode parentNode;

    private List<Quest> quests = new List<Quest>();

    private void Awake()
    {
        // Singleton pattern to ensure only one instance of DialogueManager
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        // Initially hide the dialogue UI
        HideDialogue();
    }

    private void OnEnable()
    {
        GameEventsManager.instance.questEvents.onQuestStateChange += QuestStateChange;
    }

    private void OnDisable()
    {
        GameEventsManager.instance.questEvents.onQuestStateChange -= QuestStateChange;
    }

    private void QuestStateChange(Quest quest)
    {
        Quest currentQuest = quests.Find(x => x.info.id == quest.info.id);
        if (currentQuest == null)
        {
            quests.Add(quest);
        }
    }

    // Starts the dialogue with given title and dialogue node
    public void StartDialogue(string title, DialogueNode node, GameObject npc)
    {
        // Turn to player
        npc.GetComponent<NPC>().RotateTowards(GameObject.FindGameObjectWithTag("Player").transform.position);
        //  Set parent dialog node
        parentNode = node;
        //  Stop all audio
        //  TODO add tags for npcs to audio source to find
        npc.GetComponent<NPC>().audioSource.Pause();
        //  Turn on dialog in hud
        HUDController.instance.EnableDialog();
        ShowDialogue(npc);
        // Display the dialogue UI
        GetDialogue(title, node, npc);
    }

    // Handles response selection and triggers next dialogue node
    public void SelectResponse(DialogueResponse response, string title, GameObject npc)
    {
        FirstPersonAudio.instance.StopDialogueAudio();
        if (response.isSkillCheck)
        {
            PlayerCharacter player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerCharacter>();
            int dice = player.characterStats.GetTotalDiceBySkill(response.skill);
            DiceManager.instance.Roll(dice, () =>
            {
                Debug.Log("dice roll finished");
                //  TODO show result of success fail or crit via new method from dice manager to determine. 1-2 success, 3 crit, all else if fail
                DiceManager.instance.ClearDice();
            });
        }
        //  Response is accepting the quest
        if (response.startQuestId != "")
        {
            GameEventsManager.instance.questEvents.StartQuest(response.startQuestId);
        }
        if (response.advancedQuestId != "")
        {
            GameEventsManager.instance.dialogEvents.ResponseRelatedQuest(response.advancedQuestId);
        }
        if (response.finishQuestId != "")
        {
            Quest quest = quests.Find(x => x.info.id == response.finishQuestId);
            //  TODO maybe find a way to only show response when CAN_FINISH
            if (quest.state == QuestState.CAN_FINISH)
            {
                GameEventsManager.instance.questEvents.FinishQuest(response.finishQuestId);
            }
        }
        // Check if there's a follow-up node
        if (!response.nextNode.IsLastNode())
        {
            GetDialogue(title, response.nextNode, npc); // Start next dialogue
        }
        else
        {
            if (response.returnToParent)
            {
                GetDialogue(title, parentNode, npc); // Start next dialogue
            }
            else
            {
                // If no follow-up node, end the dialogue
                HideDialogue();
                npc.GetComponent<NPC>().ResetRotation();
                npc.GetComponent<NPC>().audioSource.UnPause();
            }
        }
    }

    private void GetDialogue(string title, DialogueNode node, GameObject npc)
    {
        // Set dialogue title and body text
        DialogTitleText.text = title;
        DialogBodyText.text = node.dialogueText;

        //  Play dialogue audio clip
        if (node.clip)
        {
            FirstPersonAudio.instance.PlayDialogueAudio(node.clip);
        }

        // Remove any existing response buttons
        foreach (Transform child in responseButtonContainer)
        {
            Destroy(child.gameObject);
        }

        // Create and setup response buttons based on current dialogue node
        foreach (DialogueResponse response in node.responses)
        {
            GameObject buttonObj = Instantiate(responseButtonPrefab, responseButtonContainer);
            buttonObj.GetComponentInChildren<TextMeshProUGUI>().text = response.responseText;

            // Setup button to trigger SelectResponse when clicked
            buttonObj.GetComponent<Button>().onClick.AddListener(() => SelectResponse(response, title, npc));
        }
    }

    // Hide the dialogue UI
    public void HideDialogue()
    {
        CameraController.instance.EndNPCInteraction();
        Cursor.lockState = CursorLockMode.Locked;
        FirstPersonLook.instance.StartMovement();
        FirstPersonMovement.instance.StartMovement();
        FirstPersonAudio.instance.StopDialogueAudio();
        DialogueParent.SetActive(false);
    }

    // Show the dialogue UI
    private void ShowDialogue(GameObject npc)
    {
        CameraController.instance.StartNPCInteraction(npc.GetComponent<NPC>().npcFocusPoint.transform);
        Cursor.lockState = CursorLockMode.None;
        FirstPersonLook.instance.StopMovement();
        FirstPersonMovement.instance.StopMovement();
        DialogueParent.SetActive(true);
    }

    // Check if dialogue is currently active
    public bool IsDialogueActive()
    {
        return DialogueParent.activeSelf;
    }
}