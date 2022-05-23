namespace ARTech.GameFramework
{
    public interface IAttackHandler
    {
        float Cooldown { get; }
        float MinAttackDistance { get; }
        float MaxAttackDistance { get; }
        bool IsPerforming { get; }
        void Attack(Character damageable);
    }
}