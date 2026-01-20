using UnityEngine;

public class DialogRelatedQuestStep : QuestStep
{
	public string questLinkId;
	private string dialogRelatedQuestId;
	private void OnEnable()
	{
		GameEventsManager.instance.dialogEvents.onResponseAdvanceQuest += AdvanceDialogQuest;
	}
	private void OnDisable()
	{
		GameEventsManager.instance.dialogEvents.onResponseAdvanceQuest -= AdvanceDialogQuest;
	}
	private void AdvanceDialogQuest(string questId)
	{

		if (questLinkId == questId)
		{
			dialogRelatedQuestId = "dialog_activated_quest_" + questId;
			FinishQuestStep();
		}
	}

	private void UpdateState()
	{
		string state = dialogRelatedQuestId.ToString();
		ChangeState(state);
	}

	protected override void SetQuestStepState(string state)
	{
		this.dialogRelatedQuestId = state;
		UpdateState();
	}
}
