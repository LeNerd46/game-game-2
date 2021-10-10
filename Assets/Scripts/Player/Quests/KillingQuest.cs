using UnityEngine;

[CreateAssetMenu(fileName = "New Killing Quest", menuName = "Quests/Killing Quest")]
public class KillingQuest : QuestBase
{
    [System.Serializable]
    public class Objectives
    {
        public EnemyProfile requiredEnemy;
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

        GameManager.instance.onEnemyDeathCallback += EnemyDeath;

        base.InitalizeQuest();
    }

    public override string GetObjectiveList()
    {
        string objectiveList = string.Empty;

        for (int i = 0; i < objectives.Length; i++)
        {
            objectiveList += $"Kill {objectives[i].requiredEnemy.enemyName} ({CurrentProgress[i]}/{RequiredAmount[i]})\n";
        }

        return objectiveList;
    }

    private void EnemyDeath(EnemyProfile enemyKilled)
    {
        for (int i = 0; i < objectives.Length; i++)
        {
            if (enemyKilled == objectives[i].requiredEnemy)
            {
                CurrentProgress[i]++;
                GameManager.instance.UpdateTracker($"Kill {objectives[i].requiredEnemy.enemyName} ({CurrentProgress[i]}/{RequiredAmount[i]})");
            }
        }

        if (CheckProgress())
            GameManager.instance.UpdateTracker($"Completed {questName}");
    }
}