using UnityEngine;

public class QuestBase : ScriptableObject
{
    public string questName;

    [TextArea(5, 15)]
    public string questDescription;

    public int[] CurrentProgress { get; set; }
    public int[] RequiredAmount { get; set; }

    public bool IsCompleted { get; set; }

    /// <summary>
    /// Starts quest and adds it to the quest log
    /// </summary>
    public virtual void InitalizeQuest()
    {
        if (GameManager.instance.objectiveTextObject == null)
            GameManager.instance.objectiveTextObject = GameObject.FindGameObjectWithTag("ObjectiveText").GetComponent<TMPro.TextMeshProUGUI>();
        if (GameManager.instance.animator == null)
            GameManager.instance.animator = GameObject.FindGameObjectWithTag("ObjectiveText").GetComponent<Animator>();

        IsCompleted = false;
        CurrentProgress = new int[RequiredAmount.Length];

        QuestLogManager.instance.AddQuest(this);
    }

    public virtual string GetObjectiveList()
    {
        return string.Empty;
    }

    /// <summary>
    /// Gets a quests current progress
    /// </summary>
    /// <returns>Whether the quest is completed or not</returns>
    public bool CheckProgress()
    {
        for (int i = 0; i < RequiredAmount.Length; i++)
        {
            if (CurrentProgress[i] < RequiredAmount[i])
                return false;
        }

        // Completed the quest
        Debug.Log($"Quest Completed({questName})");
        return true;
    }
}
