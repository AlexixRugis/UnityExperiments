namespace ARTech.GameFramework
{
    public interface ARTGF_IAttackHandler
    {
        float Cooldown { get; }
        float MinAttackDistance { get; }
        float MaxAttackDistance { get; }
        bool IsPerforming { get; }
        void Attack(ARTGF_Character damageable);
    }
}