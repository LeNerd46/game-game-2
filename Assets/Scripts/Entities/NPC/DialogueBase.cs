using UnityEngine;

[CreateAssetMenu(fileName = "New Dialogue", menuName = "Dialogue/Dialogue")]
public class DialogueBase : ScriptableObject
{
    [System.Serializable]
    public class Info
    {
        public string characterName;

        [TextArea(3, 15)]
        public string text;
    }

    public Info[] dialogue;

    /// <summary>
    /// Adds this dialogue to the queue
    /// </summary>
    public void Enqueue()
    {
        DialogueManager.instance.EnqueueDialogue(this);
    }
}
