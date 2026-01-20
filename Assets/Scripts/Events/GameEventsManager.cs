using UnityEngine;

public class GameEventsManager : MonoBehaviour
{
    public static GameEventsManager instance { get; private set; }
    public QuestEvents questEvents;
    public DialogueEvents dialogEvents;
    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("Found more than one game events manager in the scene");
        }
        instance = this;
        questEvents = new QuestEvents();
        dialogEvents = new DialogueEvents();
    }
}
