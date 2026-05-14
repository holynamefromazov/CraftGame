using UnityEngine;
using UnityEngine.Assertions;

public class PlayerEquipment : MonoBehaviour
{
    [SerializeField] private PlayerInventory _inventory;
    public IWeapon CurrentWeapon { get; private set; }
    private void Start()
    {
        Assert.IsNotNull(_inventory, "PlayerInventory reference is not assigned.");
    }
    public void EquipWeapon(IWeapon weapon)
    {
        if (weapon != null)
        {
            CurrentWeapon = weapon;
        }
        else
        {
            Debug.LogError("Attempted to equip a null weapon.");
        }
    }
}
