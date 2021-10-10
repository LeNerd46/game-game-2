using UnityEngine;

[System.Serializable]
public enum ItemType
{
    Armor,
    Weapon,
    Default
}

[System.Serializable]
public enum InteractEvent
{
    PickUp,
    Open,
    Wear,
    Talk
}

[System.Serializable]
public enum InteractType
{
    Item,
    NPC
}

[System.Serializable]
public class Item : ScriptableObject
{
    // public GameObject prefab;

    public ItemType type;
    public InteractEvent interactEvent;
    public InteractType interactType;

    public string itemName;

    [TextArea(5, 15)]
    public string description;

    public void Awake()
    {
        interactType = InteractType.Item;
        itemName = name;
    }

    public void OnValidate()
    {   
        itemName = name;
    }

    public void OnEnable()
    {
        itemName = name;
    }
}
