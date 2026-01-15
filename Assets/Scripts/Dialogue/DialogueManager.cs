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
    private Quaternion originalRotation;

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

    // Starts the dialogue with given title and dialogue node
    public void StartDialogue(string title, DialogueNode node, GameObject npc)
    {
        // Turn to player
        originalRotation = npc.transform.rotation;
        Transform player = GameObject.FindGameObjectWithTag("Player").transform;
        Vector3 targetPosition = new Vector3(player.position.x, transform.position.y, player.position.z);
        npc.transform.LookAt(targetPosition);
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
                npc.transform.rotation = originalRotation;
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
        DialogueParent.SetActive(false);
    }

    // Show the dialogue UI
    private void ShowDialogue(GameObject npc)
    {
        //  TODO get closest gameobject interactable
        //  Probably not good, but this works for now.
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