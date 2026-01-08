using UnityEngine;

[CreateAssetMenu(fileName = "New Dialogue", menuName = "Dialogue/Dialogue Asset")]
public class Dialogue : ScriptableObject
{
	//First node of the conversation
	public DialogueNode RootNode;
	public void StartDialogue()
	{
		DialogueManager.Instance.StartDialogue("test", RootNode);
	}
}