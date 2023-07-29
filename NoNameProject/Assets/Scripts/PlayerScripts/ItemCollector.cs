using System;
using PlayerScripts;
using UnityEngine;
using UnityEngine.UI;

public class ItemCollector : MonoBehaviour
{
    [SerializeField] private int coins;
    [SerializeField] private Text coinsText;

    private PlayerHealth playerHealth;
    
    private void Start()
    {
        playerHealth = gameObject.GetComponent<PlayerHealth>();
    }


    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Coin"))
        {
            Destroy(col.gameObject);
            coins = coins + col.gameObject.GetComponent<Coin>().coinAmount;
            coinsText.text = "Coins: " + coins;
        }

        if (col.gameObject.CompareTag("Heart"))
        {
            var healthAmount = col.gameObject.GetComponent<Heart>().healthAmount;
            
            Destroy(col.gameObject);
            playerHealth.AddHealth(healthAmount);
        }
        
        if (col.gameObject.CompareTag("Weapon"))
        {
            var weaponSO = col.gameObject.GetComponent<Weapon>().GetWeaponSO();
            
            gameObject.GetComponent<PlayerCombat>().AddWeapon(weaponSO);
            Destroy(col.gameObject);
        }
    }
}
