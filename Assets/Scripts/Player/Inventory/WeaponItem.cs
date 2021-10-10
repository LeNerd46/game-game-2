using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon Item", menuName = "Items/Weapon")]
public class WeaponItem : Item
{
    public int damage;

    public new void Awake()
    {
        type = ItemType.Weapon;
    }
}
