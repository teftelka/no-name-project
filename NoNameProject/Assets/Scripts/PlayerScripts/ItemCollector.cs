using System;
using PlayerScripts;
using UnityEngine;
using UnityEngine.UI;

public class ItemCollector : MonoBehaviour
{
    [SerializeField] private int coins;
    [SerializeField] private Text coinsText;

    private PlayerHealth playerHealth;
    private GameInput gameInput;
    
    private void Start()
    {
        playerHealth = gameObject.GetComponent<PlayerHealth>();
        gameInput = FindObjectOfType<GameInput>();
    }


    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Coin"))
        {
            coins = coins + col.gameObject.GetComponent<Coin>().coinAmount;
            coinsText.text = "Coins: " + coins;
            Destroy(col.gameObject);
        }

        if (col.gameObject.CompareTag("Heart"))
        {
            var healthAmount = col.gameObject.GetComponent<Heart>().healthAmount;
            playerHealth.AddHealth(healthAmount);
            Destroy(col.gameObject);
        }
        
        if (col.gameObject.CompareTag("Weapon"))
        {
            var weaponSO = col.gameObject.GetComponent<Weapon>().GetWeaponSO();
            
            gameObject.GetComponent<PlayerCombat>().AddWeapon(weaponSO);
            Destroy(col.gameObject);
        }
        
        if (col.gameObject.CompareTag("Sphere"))
        {
            gameInput.StartDistanceAttack();
            Destroy(col.gameObject);
        }
    }
}
