using UnityEngine;

[CreateAssetMenu(fileName = "WeaponItem", menuName = "My Objects/WeaponItem")]
public class WeaponItem : BaseItem, IWeapon
{
    [SerializeField] private float damage;
    public float Damage => damage;
    [SerializeField] private float range;
    public float Range => range;
}
