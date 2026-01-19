using UnityEngine;

public class GameEventsManager : MonoBehaviour
{
    public static GameEventsManager instance { get; private set; }
    public QuestEvents questEvents;
    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("Found more than one game events manager in the scene");
        }
        instance = this;
        questEvents = new QuestEvents();
    }
}
