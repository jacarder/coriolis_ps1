// using UnityEngine;

// public class TestedTalkingStep : QuestStep
// {
// 	public string questLinkId;
// 	private string dialogActivatedQuestId;
// 	private void OnEnable()
// 	{
// 		GameEventsManager.instance.dialogEvents.onResponseAcceptsQuest += StartDialogQuest;
// 	}
// 	private void OnDisable()
// 	{
// 		GameEventsManager.instance.dialogEvents.onResponseAcceptsQuest -= StartDialogQuest;
// 	}
// 	private void StartDialogQuest(string questId)
// 	{

// 		if (questLinkId == questId)
// 		{
// 			dialogActivatedQuestId = "dialog_activated_quest_" + questId;
// 			FinishQuestStep();
// 		}
// 	}

// 	private void UpdateState()
// 	{
// 		string state = dialogActivatedQuestId.ToString();
// 		ChangeState(state);
// 	}

// 	protected override void SetQuestStepState(string state)
// 	{
// 		this.dialogActivatedQuestId = state;
// 		UpdateState();
// 	}
// }
