using System.Collections;
using System.Collections.Generic;
using DigitalRuby.AdvancedPolygonCollider;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private int health;
    private int maxHealth = 300000;

    private Animator animator;
    public bool IsDead;
    private AdvancedPolygonCollider _collider;
    
    void Start()
    {
        health = maxHealth;
        animator = GetComponent<Animator>();
        _collider = GetComponent<AdvancedPolygonCollider>();
    }

    public void TakeDamage(int damageAmount, bool isCriticalDamage)
    {
        if (!IsDead)
        {
            health -= damageAmount;
            _collider.RunInPlayMode = false;
            animator.SetTrigger("Hurt");
            DamagePopup.Create(transform.position, damageAmount, isCriticalDamage);

            if (health <= 0)
            {
                IsDead = true;
                _collider.RunInPlayMode = false;
                animator.SetBool("IsDead", true);
                gameObject.GetComponent<EnemyPatrol>().StopPatrolling();
            }
        }
    }

    private void EnableRunInPlayModeCollider()
    {
        //_collider.RunInPlayMode = true;
    }
    
    private void Death()
    {
        Destroy(gameObject);
    }
}
