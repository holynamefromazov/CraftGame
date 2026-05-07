using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    [SerializeField]
    private List<InventorySlot> inventorySlots = new();
    public float TotalWeight => CalculateTotalWeight();
    public event System.Action OnInventoryChanged;

    private float CalculateTotalWeight()
    {
        float totalWeight = 0f;
        foreach (var slot in inventorySlots)
        {
            totalWeight += slot.Item.Weight * slot.Quantity;
        }
        return totalWeight;
    }
    public void AddItem(BaseItem item, int quantity = 1)
    {
        if (item == null)
        {
            Debug.LogWarning("Cannot add a null item to the inventory.");
            return;
        }

        var existingSlot = inventorySlots.Find(slot => slot.Item.ID == item.ID);
        if (existingSlot != null)
        {
            existingSlot.AddQuantity(quantity);
        }
        else
        {
            inventorySlots.Add(new InventorySlot(item, quantity));
        }
        OnInventoryChanged?.Invoke();
    }
    public void RemoveItem(BaseItem item, int quantity = 1)
    {
        if (item == null)
        {
            Debug.LogWarning("Cannot remove a null item from the inventory.");
            return;
        }

        var existingSlot = inventorySlots.Find(slot => slot.Item.ID == item.ID);
        if (existingSlot != null)
        {
            existingSlot.RemoveQuantity(quantity);
            if (existingSlot.Quantity <= 0)
            {
                inventorySlots.Remove(existingSlot);
            }
        }
        else
        {
            Debug.LogWarning($"Item '{item.ItemName}' not found in inventory.");
        }
        OnInventoryChanged?.Invoke();
    }

    public BaseItem SearchItemByID(string id)
    {
        var slot = inventorySlots.Find(s => s.Item.ID == id);
        return slot != null ? slot.Item : null;
    }

    public void ClearSlot(BaseItem item)
    {
        if (item == null)
        {
            Debug.LogWarning("Cannot clear a null item from the inventory.");
            return;
        }

        var existingSlot = inventorySlots.Find(slot => slot.Item.ID == item.ID);
        if (existingSlot != null)
        {
            inventorySlots.Remove(existingSlot);
        }
        else
        {
            Debug.LogWarning($"Item '{item.ItemName}' not found in inventory.");
        }
        OnInventoryChanged?.Invoke();
    }
    public void ClearSlot(InventorySlot slot)
    {
        if (slot == null)
        {
            Debug.LogWarning("Cannot clear a null inventory slot.");
            return;
        }

        if (inventorySlots.Contains(slot))
        {
            inventorySlots.Remove(slot);
        }
        else
        {
            Debug.LogWarning("The specified inventory slot is not in the inventory.");
        }
        OnInventoryChanged?.Invoke();
    }
    public void ClearInventory()
    {
        inventorySlots.Clear();
        OnInventoryChanged?.Invoke();
    }
}
