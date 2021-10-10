using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public int health;
    public int maxHealth;

    public Vector3 position;
    public Quaternion rotation;
}

[System.Serializable]
public class ItemData
{
    /*public ItemType type;
    public InteractEvent interactEvent;
    public InteractType interactType;

    public string itemName;
    public string description;*/

    public string json;

    // public int amount;
}