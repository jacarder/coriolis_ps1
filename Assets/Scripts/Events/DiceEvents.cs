using System;
using UnityEngine;

public class DiceEvents
{

	public event Action<DiceResult> onDiceRollResponse;
	public void DiceRollResponse(DiceResult diceResult)
	{
		if (onDiceRollResponse != null)
		{
			onDiceRollResponse(diceResult);
		}
	}
}
