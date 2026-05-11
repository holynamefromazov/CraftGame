public interface IDamageable
{
    public float Durability { get; }
    public bool TakeDamage(float amount);
}
