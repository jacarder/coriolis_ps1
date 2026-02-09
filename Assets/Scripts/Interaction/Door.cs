using UnityEngine;

public class Door : MonoBehaviour
{
    [Header("Scene")]
    public string sceneToLoad;
    private DoorTransition doorTransitionScript;
    void Awake()
    {
        doorTransitionScript = GetComponentInChildren<DoorTransition>();
    }
    public void SceneTransition()
    {
        doorTransitionScript.ActivateDoor(sceneToLoad);
    }
}
