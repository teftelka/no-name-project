using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;

public class Sphere : MonoBehaviour
{
    private Vector3 shootDir;
    [SerializeField] private int sphereDamage = 100;
    List<Enemy> enemies = new List<Enemy>();
    
    private Color criticalHitSphereColor = UtilsClass.GetColorFromString("0EF4FF");
    
    public void Setup(Vector3 shootDir, bool isCriticalHit)
    {
        this.shootDir = shootDir;
        if (isCriticalHit)
        {
            sphereDamage *= 2;
            gameObject.GetComponentInChildren<SpriteRenderer>().color = criticalHitSphereColor;
        }
        
        Destroy(gameObject, 1f);
    }

    private void Update()
    {
        float moveSpeed = 30f;
        transform.position += shootDir * moveSpeed * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        Enemy enemy = col.GetComponent<Enemy>();

        if (enemy != null && !enemies.Contains(enemy))
        {
            enemy.TakeDamage(sphereDamage, false);
            Destroy(gameObject);
        }
        
        enemies.Add(enemy);
    }
}
