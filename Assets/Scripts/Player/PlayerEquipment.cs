using UnityEngine;

public class PlayerEquipment : MonoBehaviour
{
    [SerializeField] private IWeapon _currentWeapon;
    public IWeapon CurrentWeapon => _currentWeapon;
    [SerializeField] private PlayerInventory _inventory;
    public void EquipWeapon(IWeapon weapon)
    {
        if (weapon != null)
        {
            _currentWeapon = weapon;
        }
        else
        {
            Debug.LogWarning("Attempted to equip a null weapon.");
        }
    }
}
