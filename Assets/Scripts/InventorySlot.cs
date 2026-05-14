using System;
using UnityEngine;

public class InventorySlot
{
    public BaseItem Item { get; private set; }
    public int Quantity { get; private set; }

    public InventorySlot(BaseItem item, int quantity = 1)
    {
        if (item == null)
        {
            throw new ArgumentNullException(nameof(item), "Item cannot be null when creating an InventorySlot.");
        }
        Item = item;

        if (quantity < 1)
        {
            Debug.LogWarning("Quantity cannot be less than 1. Setting quantity to 1.");
            Quantity = 1;
            return;
        }
        Quantity = quantity;
    }

    public void AddQuantity(int amount)
    {
        if (amount < 0)
        {
            Debug.LogWarning("Cannot add a negative quantity.");
            return;
        }

        Quantity += amount;
    }

    public void RemoveQuantity(int amount)
    {
        if (amount < 0)
        {
            Debug.LogWarning("Cannot remove a negative quantity.");
            return;
        }

        Quantity -= amount;
    }

    public void SetQuantity(int newQuantity)
    {
        if (newQuantity < 0)
        {
            Debug.LogWarning("Quantity cannot be set to a negative value.");
            return;
        }

        Quantity = newQuantity;
    }
}
