using System;
using System.Collections;
using System.Collections.Generic;
using DigitalRuby.AdvancedPolygonCollider;
using PlayerScripts;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private int health;
    [SerializeField] private int maxHealth = 300;
    [SerializeField] private int damage = 30;

    private Animator animator;
    public bool IsDead;

    private GameObject player;

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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            player = collision.gameObject;
            player.GetComponent<PlayerHealth>().TakeDamage(damage);
            
            //откинуть игрока при касании
            Vector2 dir = collision.contacts[0].point - (Vector2)transform.position;
            dir = dir.normalized;
            player.GetComponent<Rigidbody2D>().AddForce(dir * 30, ForceMode2D.Impulse);
        }
    }


}
