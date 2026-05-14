using UnityEngine;

public class CollectableObject : MonoBehaviour, ICollectable
{
    [SerializeField] private ItemComponent _itemComponent;
    private void Awake()
    {
        if (_itemComponent == null && !TryGetComponent(out _itemComponent))
        {
            Debug.LogError("ItemComponent not found in " + gameObject.name, this);
        }
        else if (_itemComponent.Item == null)
        {
            Debug.LogError("BaseItem not assigned in ItemComponent of " + gameObject.name, this);
        }
    }
    public bool Collect(PlayerInventory inventory)
    {
        if (_itemComponent == null)
        {
            Debug.LogError("ItemComponent not found in " + gameObject.name, this);
            return false;
        }
        else if (_itemComponent.Item == null)
        {
            Debug.LogError("BaseItem not assigned in ItemComponent of " + gameObject.name, this);
            return false;
        }
        inventory.AddItem(_itemComponent.Item);
        Destroy(gameObject);
        return true;
    }
}
