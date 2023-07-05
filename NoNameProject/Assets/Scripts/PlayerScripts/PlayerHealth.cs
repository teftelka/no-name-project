using UnityEngine;
using UnityEngine.Serialization;

namespace PlayerScripts
{
    public class PlayerHealth : MonoBehaviour
    {
        private int maxHealth = 200;
        private int currentHealth;

        [SerializeField] private HealthBar healthBar;
        
        void Start()
        {
            currentHealth = maxHealth;
            healthBar.SetMaxHealth(maxHealth);
        }
        
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                TakeDamage(20);
            }
        
        }

        private void TakeDamage(int damage)
        {
            currentHealth -= damage;
            healthBar.SetHealth(currentHealth);
        }
    }
}
