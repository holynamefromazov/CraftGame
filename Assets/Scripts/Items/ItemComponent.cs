using UnityEngine;

public class ItemComponent : MonoBehaviour
{
    [SerializeField] private BaseItem _item;
    public BaseItem Item => _item;
}
