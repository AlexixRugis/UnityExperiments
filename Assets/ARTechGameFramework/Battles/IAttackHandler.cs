namespace ARTech.GameFramework
{
    public interface IAttackHandler
    {
        float Cooldown { get; }
        float AttackDistance { get; }
        bool IsPerforming { get; }
        void Attack(Character damageable);
    }
}