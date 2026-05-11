using UnityEngine;

public class CollectableObject : MonoBehaviour, ICollectable
{
    [SerializeField] private ItemComponent _itemComponent;
    private void Awake()
    {
        if (_itemComponent == null && !TryGetComponent(out _itemComponent))
        {
            Debug.Log("ItemComponent not found in " + gameObject.name);
        }
        else if (_itemComponent.Item == null)
        {
            Debug.Log("BaseItem not assigned in ItemComponent of " + gameObject.name);
        }
    }
    public bool Collect(PlayerInventory inventory)
    {
        if (_itemComponent == null)
        {
            Debug.Log("ItemComponent not found in " + gameObject.name);
            return false;
        }
        else if (_itemComponent.Item == null)
        {
            Debug.Log("BaseItem not assigned in ItemComponent of " + gameObject.name);
            return false;
        }
        inventory.AddItem(_itemComponent.Item);
        Destroy(gameObject);
        return true;
    }
}
