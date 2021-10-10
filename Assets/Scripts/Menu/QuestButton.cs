using UnityEngine;
using TMPro;

public class QuestButton : MonoBehaviour
{
    [HideInInspector]
    public QuestBase quest;

    public TextMeshProUGUI nameText;

    public void SetQuest(QuestBase quest)
    {
        this.quest = quest;
        nameText.text = quest.questName;
    }

    public void OnClick()
    {
        QuestLogManager.instance.UpdateQuestUI(quest, quest.GetObjectiveList());
    }
}