namespace ARTech.GameFramework
{
    public interface IDamageable : ITransformable
    {
        void TakeDamage(float amount);
        float Health { get; set; }
        float MaxHealth { get; set; }
        void ResetHealth();
    }
}