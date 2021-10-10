using UnityEngine;

public class Interactable : MonoBehaviour
{
    public Item item;
    public NPC npc;
    public InventoryObject inventory;

    public DialogueBase dialogueBase;

    public InteractType interactType;

    public GameObject UI;
    public GameObject openUI;

    public void PickUp()
    {
        if (GameManager.instance.onItemPickup != null)
            GameManager.instance.onItemPickup.Invoke(item);

        inventory.AddItem(item, 1);

        Destroy(gameObject);
    }

    public void Open()
    {
        UI.SetActive(false);
        openUI.SetActive(true);
    }

    public void Wear()
    {
        // PlayerHealth.armorPoints += item.armorValue;
        Destroy(gameObject);
    }

    public void Talk(DialogueBase dialogue)
    {
        DialogueManager.instance.EnqueueDialogue(dialogue);
    }
}
