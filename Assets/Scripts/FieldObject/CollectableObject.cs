using UnityEngine;

public class CollectableObject : MonoBehaviour, ICollectable
{
    [SerializeField] private ItemComponent itemComponent;
    private void Awake()
    {
        if (itemComponent == null && !TryGetComponent(out itemComponent))
        {
            Debug.Log("ItemComponent not found in " + gameObject.name);
        }
        else if (itemComponent.Item == null)
        {
            Debug.Log("BaseItem not assigned in ItemComponent of " + gameObject.name);
        }
    }
    public bool Collect(PlayerInventory inventory)
    {
        if (itemComponent == null)
        {
            Debug.Log("ItemComponent not found in " + gameObject.name);
            return false;
        }
        else if (itemComponent.Item == null)
        {
            Debug.Log("BaseItem not assigned in ItemComponent of " + gameObject.name);
            return false;
        }
        inventory.AddItem(itemComponent.Item);
        Destroy(gameObject);
        return true;
    }
}
