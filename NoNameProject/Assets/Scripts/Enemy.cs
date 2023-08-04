using System;
using PlayerScripts;
using UnityEngine;
using Random = System.Random;

public class Enemy : MonoBehaviour
{
    private int health;
    private int maxHealth;
    private int damage;

    [SerializeField] private EnemySO enemySo;
    
    private Animator animator;
    private bool isDead;

    private GameObject player;

    [SerializeField] private GameObject coin;
    [SerializeField] private GameObject heart;

    private ScoreManager scoreManager;

    [SerializeField] private GameObject pfHealthBar;
    private GameObject healthBar;
    private EnemyHealthBar enemyHealthBar;

    private float armorRate;
    private bool isArmor;
    private string armorType;
    private float currentArmorRate;
    private float armorPercentage;

    private void Awake()
    {
        scoreManager = FindObjectOfType<ScoreManager>();
        healthBar = Instantiate(pfHealthBar, transform.position + new Vector3(0, 2.5f), Quaternion.identity, transform);

        enemyHealthBar = healthBar.GetComponent<EnemyHealthBar>();
    }

    void Start()
    {
        maxHealth = enemySo.maxHealth;
        damage = enemySo.damage;
        isArmor = enemySo.isArmor;
        if (isArmor)
        {
            armorRate = enemySo.armorRate;
            armorType = enemySo.armorType;
            SetArmor();
        }

        health = maxHealth;
        animator = GetComponent<Animator>();
    }

    public void TakeDamage(int damageAmount, bool isCriticalDamage, bool meleeAttack)
    {
        if (!isDead)
        {
            if (isArmor)
            {
                if (meleeAttack && armorType == "melee" || !meleeAttack && armorType == "distance")
                {
                    ArmorDamage(damageAmount);
                    DamagePopup.Create(transform.position, damageAmount, false);
                }
            }

            else
            {
                health -= damageAmount;
                animator.SetTrigger("Hurt");
                DamagePopup.Create(transform.position, damageAmount, isCriticalDamage);

                HealthBarChanged();

                if (health <= 0)
                {
                    isDead = true;
                    healthBar.SetActive(false);
                
                    animator.SetBool("IsDead", true);
                    gameObject.GetComponent<EnemyPatrol>().StopPatrolling();
                
                    Invoke(nameof(InstantiateItem), 0.5f);
                }
            }
        }
    }
    
    private void ArmorDamage(int damageAmount)
    {
        currentArmorRate -= damageAmount;
            
        armorPercentage = currentArmorRate / maxHealth;
        if (armorPercentage < 0)
        {
            armorPercentage = 0;
            isArmor = false;
        }
        enemyHealthBar.SetArmorSize(armorPercentage); 
    }

    private void HealthBarChanged()
    {
        float healthPercentage = (float)health / maxHealth;
        if (healthPercentage < 0) healthPercentage = 0;
        enemyHealthBar.SetSize(healthPercentage);
    }

    private void SetArmor()
    {
        currentArmorRate = armorRate;
        armorPercentage = armorRate / maxHealth;
        enemyHealthBar.SetArmorSize(armorPercentage);
        
        enemyHealthBar.SetArmorColor(armorType);
    }
    

    public bool IsDead()
    {
        return isDead;
    }

    private void Death()
    {
        scoreManager.AddPoints(1);
        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            player = collision.gameObject;
            player.GetComponent<PlayerHealth>().TakeDamage(damage);
            
            //откинуть игрока при касании
            Vector2 dir = collision.contacts[0].point - (Vector2)transform.position;
            dir = dir.normalized;
            player.GetComponent<Rigidbody2D>().AddForce(dir * 15, ForceMode2D.Impulse);
        }
    }

    private void InstantiateItem()
    {
        var random = new Random();
        if (random.Next(0,2) == 0)
        {
            Instantiate(coin, gameObject.transform.position, Quaternion.identity);
        }
        else
        {
            Instantiate(heart, gameObject.transform.position, Quaternion.identity);
        }
    }

    public Transform GetHealthBar()
    {
        return healthBar.transform;
    }
}
