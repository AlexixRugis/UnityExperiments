namespace ARTech.GameFramework
{
    public interface IDamageable
    {
        void TakeDamage(float amount);
        float Health { get; set; }
        float MaxHealth { get; set; }
        void ResetHealth();
    }
}