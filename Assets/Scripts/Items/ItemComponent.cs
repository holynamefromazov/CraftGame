using UnityEngine;

public class ItemComponent : MonoBehaviour
{
    [SerializeField] private BaseItem item;
    public BaseItem Item => item;
}
