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
        
        public void TakeDamage(int damage)
        {
            currentHealth -= damage;
            healthBar.SetHealth(currentHealth);
            if (currentHealth < 0)
            {
                FindObjectOfType<GameManager>().EndGame();
            }
        }

        public void AddHealth(int healthAmount)
        {
            currentHealth = currentHealth + healthAmount;
            
            if (currentHealth > maxHealth)
            {
                currentHealth = maxHealth;
            }
            healthBar.SetHealth(currentHealth);
        }
    }
}
