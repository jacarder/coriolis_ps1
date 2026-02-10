using UnityEngine;

public class Door : MonoBehaviour
{
    [Header("Scene")]
    public string sceneToLoad;
    public Vector3 nextScenePosition;
    public Quaternion nextSceneRotation;
    private DoorTransition doorTransitionScript;

    void Awake()
    {
        doorTransitionScript = GetComponentInChildren<DoorTransition>();
    }
    public void SceneTransition()
    {
        doorTransitionScript.ActivateDoor(sceneToLoad, nextScenePosition, nextSceneRotation);
    }
}
