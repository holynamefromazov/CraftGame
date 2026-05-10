using UnityEngine;

public class PlayerEquipment : MonoBehaviour
{
    [SerializeField] private IWeapon currentWeapon;
    public IWeapon CurrentWeapon => currentWeapon;
    [SerializeField] private PlayerInventory inventory;
    public void EquipWeapon(IWeapon weapon)
    {
        if (weapon != null)
        {
            currentWeapon = weapon;
        }
        else
        {
            Debug.LogWarning("Attempted to equip a null weapon.");
        }
    }
}
