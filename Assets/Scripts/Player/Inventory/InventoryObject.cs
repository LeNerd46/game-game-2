using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Inventory", menuName = "Inventory")]
public class InventoryObject : ScriptableObject
{
    public List<InventorySlot> container = new List<InventorySlot>();

    /// <summary>
    /// Adds an item to the inventory
    /// </summary>
    /// <param name="_item">Item to add</param>
    /// <param name="_amount">Amount of items to add</param>
    public void AddItem(Item _item, int _amount)
    {
        bool hasItem = false;

        for (int i = 0; i < container.Count; i++)
        {
            if(container[i].item == _item)
            {
                container[i].AddAmount(_amount);

                hasItem = true;
                break;
            }
        }

        if(!hasItem)
            container.Add(new InventorySlot(_item, _amount));
    }
}

[System.Serializable]
public class InventorySlot
{
    public Item item;
    public int amount;

    public InventorySlot(Item _item, int _amount)
    {
        item = _item;
        amount = _amount;
    }

    public void AddAmount(int value)
    {
        amount += value;
    }
}