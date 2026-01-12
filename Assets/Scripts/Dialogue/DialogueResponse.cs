using UnityEngine.WSA;

[System.Serializable]
public class DialogueResponse
{
	public string responseText;
	public DialogueNode nextNode;
	public bool returnToParent = false;
}