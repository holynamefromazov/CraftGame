using UnityEngine;

public class DamageableObject : MonoBehaviour, IDamageable
{
    [SerializeField] private float _durability = 100f;
    public float Durability => _durability;

    public bool TakeDamage(float amount)
    {
        _durability -= amount;
        if (_durability <= 0)
        {
            Destroy(gameObject);
            return true;
        }
        return false;
    }
}
