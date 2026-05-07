using UnityEngine;

public enum ItemCategory
{
    Consumable,
    Weapon,
    Tool,
    Resource,
    Quest
}

[CreateAssetMenu(fileName = "BaseItem", menuName = "My Objects/BaseItem")]
public class BaseItem : ScriptableObject
{
    [Header("Base Item Properties")]
    [SerializeField]
    private string id;
    public string ID => id;
    [SerializeField]
    private string itemName;
    public string ItemName => itemName;
    [SerializeField]
    protected ItemCategory category;
    public ItemCategory Category => category;
    [SerializeField]
    private Sprite itemIcon;
    public Sprite ItemIcon => itemIcon;
    [SerializeField]
    private float weight = 0.1f;
    public float Weight => weight;

#if UNITY_EDITOR
    private void OnEnable()
    {
        IdAndNameSetup();
    }
    protected void OnValidate()
    {
        IdAndNameSetup();

        if (itemIcon == null)
        {
            Debug.LogWarning($"{this.GetType()} '{this.name}' does not have an icon assigned.");
        }

        if (weight < 0)
        {
            Debug.LogWarning($"{this.GetType()} '{this.name}' has a negative weight ({weight}). Weight should be a non-negative value.");
        }
    }
    private void IdAndNameSetup()
    {
        itemName = string.IsNullOrEmpty(itemName) ? this.name : itemName;
        if (string.IsNullOrEmpty(id))
        {
            id = itemName.ToLower().Replace(" ", "_");
        }
        else
        {
            id = id.ToLower().Replace(" ", "_");
        }
    }
#endif
}
