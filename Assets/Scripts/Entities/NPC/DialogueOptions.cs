using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "New Dialogue Options", menuName = "Dialogue/Dialogue Options")]
public class DialogueOptions : DialogueBase
{
    [System.Serializable]
    public class Options
    {
        public DialogueBase nextDialogue;
        public QuestBase giveQuest;

        public string optionName;
    }

    public Options[] options;
}
