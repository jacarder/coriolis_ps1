using UnityEngine;

[CreateAssetMenu(fileName = "New Dialogue", menuName = "Dialogue/Dialogue Asset")]
public class Dialogue : ScriptableObject
{
	//First node of the conversation
	public DialogueNode RootNode;
	public void StartDialogue()
	{
		//	TODO how do we get the npc name. Cant do it here because this is a scriptable object
		DialogueManager.Instance.StartDialogue("", RootNode);
	}
}