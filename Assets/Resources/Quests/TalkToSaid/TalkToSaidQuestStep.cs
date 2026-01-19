using UnityEngine;

public class TalkToSaidQuestStep : QuestStep
{
    private string npcToTalkTo = "Said";
    private bool hasTalked = false;
    private void OnEnabled() { }
    private void NPCTalkedTo()
    {
        if (hasTalked)
        {
            FinishQuestStep();
        }
    }

    private void UpdateState()
    {
        string state = npcToTalkTo.ToString();
        ChangeState(state);
    }

    protected override void SetQuestStepState(string state)
    {
        this.hasTalked = System.Boolean.Parse(state);
        UpdateState();
    }
}
