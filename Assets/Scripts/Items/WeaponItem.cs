using UnityEngine;

[CreateAssetMenu(fileName = "WeaponItem", menuName = "My Objects/WeaponItem")]
public class WeaponItem : BaseItem, IWeapon
{
    [Header("Weapon Item Properties")]
    [SerializeField] private float _damage;
    public float Damage => _damage;
    [SerializeField] private float _range;
    public float Range => _range;

    void OnEnable()
    {
        _category = ItemCategory.Weapon;
    }

}
