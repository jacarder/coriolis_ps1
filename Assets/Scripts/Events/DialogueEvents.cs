using System;
using UnityEngine;

public class DialogueEvents
{
	public event Action<string> onResponseAdvanceQuest;
	public void ResponseRelatedQuest(string questId)
	{
		if (onResponseAdvanceQuest != null)
		{
			onResponseAdvanceQuest(questId);
		}
	}
}
