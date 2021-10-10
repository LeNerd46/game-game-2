using UnityEngine;

[CreateAssetMenu(fileName = "New Fetch Quest", menuName = "Quests/Fetch Quest")]
public class FetchQuest : QuestBase
{
    [System.Serializable]
    public class Objectives
    {
        public Item requiredItem;
        public int requiredAmount;
    }

    public Objectives[] objectives;

    public override void InitalizeQuest()
    {
        RequiredAmount = new int[objectives.Length];

        for (int i = 0; i < objectives.Length; i++)
        {
            RequiredAmount[i] = objectives[i].requiredAmount;
        }

        GameManager.instance.onItemPickup += ItemPickedUp;

        base.InitalizeQuest();
    }

    public override string GetObjectiveList()
    {
        string objectiveList = string.Empty;

        for (int i = 0; i < objectives.Length; i++)
        {
            objectiveList += $"Collect {objectives[i].requiredItem.itemName} ({CurrentProgress[i]}/{RequiredAmount[i]})\n";
        }

        return objectiveList;
    }

    private void ItemPickedUp(Item itemPickedUp)
    {
        for (int i = 0; i < objectives.Length; i++)
        {
            if (itemPickedUp == objectives[i].requiredItem)
            {
                CurrentProgress[i]++;
                GameManager.instance.UpdateTracker($"Collect {objectives[i].requiredItem.itemName} ({CurrentProgress[i]}/{RequiredAmount[i]})");
            }
        }

        if (CheckProgress() && !IsCompleted)
        {
            GameManager.instance.UpdateTracker($"Completed {questName}");
            IsCompleted = true;
        }
    }
}
