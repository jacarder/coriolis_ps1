using UnityEngine.WSA;

[System.Serializable]
public class DialogueResponse
{
	public string responseText;
	public DialogueNode nextNode;
	public bool isSkillCheck;
	public string startQuestId;
	public string advancedQuestId;
	public string finishQuestId;
	public SkillType skill;
	public bool returnToParent = false;
}