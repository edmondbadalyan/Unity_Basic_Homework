using UnityEngine;

namespace Assets.Scipts
{
    public class DamageCalculator
    {
        public void CalculateDamage(float baseDamage, int damageMultiplier)
        {
            var result = baseDamage * damageMultiplier;
            Debug.LogWarning($"Calculated Damage: {baseDamage} * {damageMultiplier} = {result}");
        }
    }
}
