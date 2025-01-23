using UnityEngine;

namespace Scipts
{
    public class Character : MonoBehaviour
    {
         private int _maxHealth = 100;
         private int _damagePerCall = 10;
         [Range(0.3f,2f)]public float _damageInterval = 1f;
         
         private int _currentHealth;
         private float _damageTimer;

         public bool IsDead;
         
         private void Awake()
         {
             _currentHealth = _maxHealth;
         }

         private void Update()
        {
            if(IsDead) return;
            
            // Урон с интервалом вместо каждого кадра
            _damageTimer += Time.deltaTime;
            
            if(_damageTimer >= _damageInterval)
            {
                ApplyDamage(_damagePerCall);
                _damageTimer = 0;
            }
        }
        public void ApplyDamage(int damage)
        {
            if(IsDead || damage <= 0) return;

            _currentHealth = Mathf.Max(_currentHealth - damage, 0);
            
            Debug.Log($"Health: {_currentHealth}");

            if(_currentHealth  <= 0 && !IsDead)
            {
                Die();
            }
        }
        private void Die()
        {
            IsDead = true;
            Debug.Log("Character died!");
        }
    }
}