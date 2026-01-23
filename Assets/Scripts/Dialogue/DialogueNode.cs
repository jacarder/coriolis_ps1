using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DialogueNode
{
	public string dialogueText;
	public string dialogueCriticalText;
	public string dialogueFailText;
	public AudioClip clip;
	public List<DialogueResponse> responses;

	internal bool IsLastNode()
	{
		return responses.Count <= 0;
	}
}