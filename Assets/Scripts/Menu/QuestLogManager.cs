using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class QuestLogManager : MonoBehaviour
{
    public static QuestLogManager instance;

    public TextMeshProUGUI questName;
    public TextMeshProUGUI questDescription;
    public TextMeshProUGUI objectiveText;

    public Transform questHolder;
    public GameObject questButtonPrefab;

    [HideInInspector]
    public QuestBase lastDisplayedQuest;

    private List<QuestBase> questsToSave;

    void Awake()
    {
        if (instance == null)
            instance = this;
    }

    void Start()
    {
        questsToSave = new List<QuestBase>();
        GameManager.instance.onGameSaved += OnSave;
        GameManager.instance.onGameLoaded += OnLoad;
    }

    /// <summary>
    /// Updates the quest logs UI to display a given quest's information
    /// </summary>
    /// <param name="quest">Quest to display</param>
    /// <param name="objectiveList">Objectives to show</param>
    public void UpdateQuestUI(QuestBase quest, string objectiveList)
    {
        lastDisplayedQuest = quest;

        questName.text = quest.questName;
        questDescription.text = quest.questDescription;
        objectiveText.text = objectiveList;
    }

    /// <summary>
    /// Adds a quest to the quest log
    /// </summary>
    /// <param name="quest">Quest to add</param>
    public void AddQuest(QuestBase quest)
    {
        var questButton = Instantiate(questButtonPrefab, questHolder);
        questButton.GetComponent<QuestButton>().SetQuest(quest);
        

        if (!questsToSave.Contains(quest))
            questsToSave.Add(quest);
    }

    private void OnSave()
    {
        foreach (var quest in questsToSave)
        {
            SaveData.Instance.questData.Add(JsonUtility.ToJson(quest));
        }
    }

    private void OnLoad()
    {
        for (int i = 0; i < questHolder.childCount; i++)
        {
            if (!questHolder.GetChild(i).name.Equals("Handle"))
                Destroy(questHolder.GetChild(i).gameObject);
        }

        foreach (var json in SaveData.Instance.questData)
        {
            QuestBase quest = ScriptableObject.CreateInstance<QuestBase>();
            JsonUtility.FromJsonOverwrite(json, quest);
            AddQuest(quest);
        }
    }
}
