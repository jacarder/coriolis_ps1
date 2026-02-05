using UnityEngine;
using UnityEngine.InputSystem;

public class KeyboardController : MonoBehaviour
{
    void Update()
    {
        if (Keyboard.current.escapeKey.wasReleasedThisFrame)
        {
            if (DialogueManager.Instance.IsDialogueActive())
            {
                DialogueManager.Instance.HideDialogue();
            }
            else if (MenuController.instance.isShowing)
            {
                MenuController.instance.HideMenu();
            }
            else
            {
                MenuController.instance.ShowMenu();
            }
        }
    }
}
