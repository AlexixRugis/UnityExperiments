using UnityEngine;

namespace ARTech.GameFramework
{
    public interface IDamageable
    {
        void TakeDamage(float amount);
        void TakeDamage(float amount, Entity source);
        Entity GetLastDamager();
        float GetHealth();
        void SetHealth(float health);
        float GetMaxHealth();
        void SetMaxHealth(float maxHealth);
        void ResetHealth();
        Vector3 GetLocation();
    }
}