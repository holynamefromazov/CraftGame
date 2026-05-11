using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    [SerializeField]
    private List<InventorySlot> _inventorySlots = new();
    public float TotalWeight { get; private set; }
    public event System.Action OnInventoryChanged;

    private void CalculateTotalWeight()
    {
        float totalWeight = 0f;
        foreach (var slot in _inventorySlots)
        {
            totalWeight += slot.Item.Weight * slot.Quantity;
        }
        TotalWeight = totalWeight;
    }
    public void AddItem(BaseItem item, int quantity = 1)
    {
        if (item == null)
        {
            Debug.LogWarning("Cannot add a null item to the inventory.");
            return;
        }

        var existingSlot = _inventorySlots.Find(slot => slot.Item.ID == item.ID);
        if (existingSlot != null)
        {
            existingSlot.AddQuantity(quantity);
        }
        else
        {
            _inventorySlots.Add(new InventorySlot(item, quantity));
        }
        CalculateTotalWeight();
        OnInventoryChanged?.Invoke();
    }
    public void RemoveItem(BaseItem item, int quantity = 1)
    {
        if (item == null)
        {
            Debug.LogWarning("Cannot remove a null item from the inventory.");
            return;
        }

        var existingSlot = _inventorySlots.Find(slot => slot.Item.ID == item.ID);
        if (existingSlot != null)
        {
            existingSlot.RemoveQuantity(quantity);
            if (existingSlot.Quantity <= 0)
            {
                _inventorySlots.Remove(existingSlot);
            }
            CalculateTotalWeight();
            OnInventoryChanged?.Invoke();
        }
        else
        {
            Debug.LogWarning($"Item '{item.ItemName}' not found in inventory.");
        }
    }

    public BaseItem SearchItemByID(string id)
    {
        var slot = _inventorySlots.Find(s => s.Item.ID == id);
        return slot?.Item;
    }

    public void ClearSlot(BaseItem item)
    {
        if (item == null)
        {
            Debug.LogWarning("Cannot clear a null item from the inventory.");
            return;
        }

        var existingSlot = _inventorySlots.Find(slot => slot.Item.ID == item.ID);
        if (existingSlot != null)
        {
            _inventorySlots.Remove(existingSlot);
            CalculateTotalWeight();
            OnInventoryChanged?.Invoke();
        }
        else
        {
            Debug.LogWarning($"Item '{item.ItemName}' not found in inventory.");
        }
    }
    public void ClearSlot(InventorySlot slot)
    {
        if (slot == null)
        {
            Debug.LogWarning("Cannot clear a null inventory slot.");
            return;
        }

        if (_inventorySlots.Contains(slot))
        {
            _inventorySlots.Remove(slot);
            CalculateTotalWeight();
            OnInventoryChanged?.Invoke();
        }
        else
        {
            Debug.LogWarning("The specified inventory slot is not in the inventory.");
        }
    }
    public void ClearInventory()
    {
        _inventorySlots.Clear();
        TotalWeight = 0f;
        OnInventoryChanged?.Invoke();
    }

#if UNITY_EDITOR

    [ContextMenu("Debug Print Inventory")]
    public void DebugPrintInventory()
    {
        Debug.Log("Current Inventory:");
        foreach (var slot in _inventorySlots)
        {
            Debug.Log($"- {slot.Item.ItemName} x{slot.Quantity} (Total Weight: {slot.Item.Weight * slot.Quantity})");
        }
        Debug.Log($"Total Inventory Weight: {TotalWeight}");
    }
#endif
}
