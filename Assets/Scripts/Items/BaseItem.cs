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
    [SerializeField] private string _id;
    public string ID => _id;
    [SerializeField] private string _itemName;
    public string ItemName => _itemName;
    [SerializeField] protected ItemCategory _category;
    public ItemCategory Category => _category;
    [SerializeField] private Sprite _itemIcon;
    public Sprite ItemIcon => _itemIcon;
    [SerializeField] private float _weight = 0.1f;
    public float Weight => _weight;
#if UNITY_EDITOR
    private void OnEnable()
    {
        IdAndNameSetup();
    }
    protected void OnValidate()
    {
        IdAndNameSetup();

        if (_itemIcon == null)
        {
            Debug.LogWarning($"{this.GetType()} '{this.name}' does not have an icon assigned.");
        }

        if (_weight < 0)
        {
            Debug.LogWarning($"{this.GetType()} '{this.name}' has a negative weight ({_weight}). Weight should be a non-negative value.");
        }
    }
    private void IdAndNameSetup()
    {
        _itemName = string.IsNullOrEmpty(_itemName) ? this.name : _itemName;
        if (string.IsNullOrEmpty(_id))
        {
            _id = _itemName.ToLower().Replace(" ", "_");
        }
        else
        {
            _id = _id.ToLower().Replace(" ", "_");
        }
    }
#endif
}
