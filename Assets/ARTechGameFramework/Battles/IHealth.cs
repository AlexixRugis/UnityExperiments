namespace ARTech.GameFramework
{
    public interface IHealth : ITransform
    {
        void TakeDamage(float amount);
        float Health { get; set; }
        float MaxHealth { get; set; }
        void ResetHealth();
    }
}