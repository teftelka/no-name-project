using System;
using PlayerScripts;
using UnityEngine;
using Random = System.Random;

public class Enemy : MonoBehaviour
{
    [SerializeField] private int health;
    [SerializeField] private int maxHealth = 300;
    [SerializeField] private int damage = 30;

    private Animator animator;
    private bool isDead;

    private GameObject player;

    [SerializeField] private GameObject coin;
    [SerializeField] private GameObject heart;

    private ScoreManager scoreManager;

    [SerializeField] private GameObject pfHealthBar;
    private GameObject healthBar;

    private void Awake()
    {
        scoreManager = FindObjectOfType<ScoreManager>();
        healthBar = Instantiate(pfHealthBar, transform.position + new Vector3(0, 2.5f), Quaternion.identity, transform);
    }
    
    void Start()
    {
        health = maxHealth;
        animator = GetComponent<Animator>();
    }

    public void TakeDamage(int damageAmount, bool isCriticalDamage)
    {
        if (!isDead)
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

    private void HealthBarChanged()
    {
        float healthPercentage = (float)health / maxHealth;
        if (healthPercentage < 0) healthPercentage = 0;
        healthBar.GetComponent<EnemyHealthBar>().SetSize(healthPercentage);
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
