using UnityEngine;

[CreateAssetMenu(fileName = "New Goto Quest", menuName = "Quests/Goto Quest")]
public class GoToQuest : QuestBase
{
    [System.Serializable]
    public class Objectives
    {
        public string locationName;
    }

    public Objectives[] objectives;

    public override void InitalizeQuest()
    {
        GameManager.instance.onLocationEntered += OnLocationEntered;
        
        base.InitalizeQuest();
    }

    public override string GetObjectiveList()
    {
        string objectivesList = string.Empty;

        for (int i = 0; i < objectives.Length; i++)
        {
            objectivesList += $"Go to {objectives[i].locationName}\n";
        }

        return objectivesList;
    }

    private void OnLocationEntered(UnityEngine.UI.Image locationImage)
    {
        for (int i = 0; i < objectives.Length; i++)
        {
            if (locationImage.GetComponent<WaypointInfo>().locationName.Equals(objectives[i].locationName))
            {
                if (CheckProgress() && !IsCompleted)
                {
                    GameManager.instance.UpdateTracker($"Found {locationImage.GetComponent<WaypointInfo>().locationName}");
                    IsCompleted = true;

                    Destroy(locationImage.gameObject);
                }
            }
        }
    }
}
