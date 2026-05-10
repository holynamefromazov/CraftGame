using UnityEngine;

public class DamageableObject : MonoBehaviour, IDamageable
{
    [SerializeField] private float durability = 100f;
    public float Durability => durability;

    public bool TakeDamage(float amount)
    {
        durability -= amount;
        if (durability <= 0)
        {
            Destroy(gameObject);
            return true;
        }
        return false;
    }
}
