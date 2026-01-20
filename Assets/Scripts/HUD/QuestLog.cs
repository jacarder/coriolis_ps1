using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuestLog : MonoBehaviour
{
    public GameObject textPrefab;
    private List<QuestLogItem> quests = new List<QuestLogItem>();

    private void OnEnable()
    {
        GameEventsManager.instance.questEvents.onQuestStateChange += QuestStateChange;
    }
    private void OnDisable()
    {
        GameEventsManager.instance.questEvents.onQuestStateChange -= QuestStateChange;
    }

    private void QuestStateChange(Quest quest)
    {
        QuestLogItem currentQuest = quests.Find(x => x.questId == quest.info.id);
        if (currentQuest == null)
        {
            quests.Add(new QuestLogItem { questId = quest.info.id });
            AddTextItem(quest.info.id, GetQuestText(quest));
        }
        else
        {
            AddTextItem(currentQuest.questId, GetQuestText(quest));
        }
    }

    private string GetQuestText(Quest quest)
    {
        string message = "";
        switch (quest.state)
        {
            case QuestState.CAN_START:
                message = "Can start quest: " + quest.info.displayName;
                break;
            case QuestState.IN_PROGRESS:
                message = "Quest in progress: " + quest.info.displayName;
                break;
            case QuestState.CAN_FINISH:
                message = "Quest can be finished: " + quest.info.displayName;
                break;
            case QuestState.FINISHED:
                message = "Quest finished: " + quest.info.displayName;
                break;
        }
        return message;
    }

    private void AddTextItem(string questId, string message)
    {
        QuestLogItem item = quests.Find(x => x.questId == questId);
        //  Update textObj
        if (item.TextObj != null)
        {
            item.TextObj.text = message;
        }
        else
        {
            GameObject newItem = Instantiate(textPrefab, this.gameObject.transform);

            TMP_Text tmp = newItem.GetComponent<TMP_Text>();
            if (tmp != null)
            {
                tmp.text = message;
            }
            else
            {
                Text uiText = newItem.GetComponent<Text>();
                if (uiText != null)
                    uiText.text = message;
            }
            item.TextObj = tmp;
        }
    }
}
