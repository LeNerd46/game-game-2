using UnityEngine;

[CreateAssetMenu(fileName = "New Default Item", menuName = "Items/Default")]
public class DefaultItem : Item
{
    public void Awake()
    {
        type = ItemType.Default;
    }
}
