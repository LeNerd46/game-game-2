using UnityEngine;

[CreateAssetMenu(fileName = "New Gun", menuName = "Weapons/Gun")]
public class GunObject : ScriptableObject
{
    public GunType gunType;

    [Header("Information")]
    public int damage;
    public int range;
    public int maxAmmo;
    public int zoomFOV;
    public int fireRate;

    /// <summary>
    /// Shoots the gun
    /// </summary>
    public void Shoot()
    {

    }
}

public enum GunType
{
    Pistol,
    Rifle,
    Sniper
}
