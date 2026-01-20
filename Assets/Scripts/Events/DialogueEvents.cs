using System;
using UnityEngine;

public class DialogueEvents
{
	// public event Action<DialogueNode> onNextNode;
	// public void NextNode(DialogueNode id)
	// {
	// 	if (onNextNode != null)
	// 	{
	// 		onNextNode(id);
	// 	}
	// }

	public event Action<string> onResponseAdvanceQuest;
	public void ResponseRelatedQuest(string questId)
	{
		if (onResponseAdvanceQuest != null)
		{
			onResponseAdvanceQuest(questId);
		}
	}
}
