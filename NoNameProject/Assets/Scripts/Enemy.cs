using System.Collections;
using System.Collections.Generic;
using DigitalRuby.AdvancedPolygonCollider;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private int health;
    private int maxHealth = 300;

    private Animator animator;
    public bool IsDead;

    void Start()
    {
        health = maxHealth;
        animator = GetComponent<Animator>();
    }

    public void TakeDamage(int damageAmount, bool isCriticalDamage)
    {
        if (!IsDead)
        {
            health -= damageAmount;
            animator.SetTrigger("Hurt");
            DamagePopup.Create(transform.position, damageAmount, isCriticalDamage);

            if (health <= 0)
            {
                IsDead = true;
                animator.SetBool("IsDead", true);
                gameObject.GetComponent<EnemyPatrol>().StopPatrolling();
            }
        }
    }

    private void Death()
    {
        Destroy(gameObject);
    }
}
