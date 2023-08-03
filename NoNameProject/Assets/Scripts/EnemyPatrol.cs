using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    [SerializeField] private GameObject pointA;
    [SerializeField] private GameObject pointB;
    private Rigidbody2D rb;
    private Animator anim;
    private Transform currentPoint;
    private int yRotation;

    private Transform healthBar;

    [SerializeField] private float speed = 2f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        
        yRotation = (int)Math.Abs(transform.eulerAngles.y);
        currentPoint = yRotation == 180 ? pointA.transform : pointB.transform;
        anim.SetBool("IsWalking", true);
        anim.SetFloat("Speed", speed);

        healthBar = gameObject.GetComponent<Enemy>().GetHealthBar();
    }

    void Update()
    {
        if (currentPoint == pointB.transform)
        {
            rb.velocity = new Vector2(speed, 0);
        }
        else
        {
            rb.velocity = new Vector2(-speed, 0);
        }

        if (currentPoint.position.x - transform.position.x < 0.5f && currentPoint == pointB.transform)
        {
            Flip(transform);
            Flip(healthBar);
            currentPoint = pointA.transform;
        }
        
        if (currentPoint.position.x - transform.position.x > -0.5f && currentPoint == pointA.transform)
        {
            Flip(transform);
            Flip(healthBar);
            currentPoint = pointB.transform;
        }
    }

    private void Flip(Transform transform)
    {
        Vector3 localScale = transform.localScale;
        localScale.x *= -1;
        transform.localScale = localScale;
    }

    public void StopPatrolling()
    {
        speed = 0;
    }

    private void OnDrawGizmos()
    {
        var positionA = pointA.transform.position;
        var positionB = pointB.transform.position;
        Gizmos.DrawWireSphere(positionA, 0.5f);
        Gizmos.DrawWireSphere(positionB, 0.5f);
        Gizmos.DrawLine(positionA, positionB);
    }
}
