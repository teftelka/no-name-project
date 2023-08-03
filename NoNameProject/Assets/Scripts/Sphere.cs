using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using CodeMonkey.Utils;

public class Sphere : MonoBehaviour
{
    private Vector3 shootDir;
    [SerializeField] private int sphereDamage = 100;
    List<Enemy> enemies = new List<Enemy>();

    [SerializeField] private Camera camera;
    
    private Color criticalHitSphereColor = UtilsClass.GetColorFromString("0EF4FF");

    private void Start()
    {
        camera = FindObjectOfType<Camera>();
    }

    public void Setup(Vector3 shootDir, bool isCriticalHit)
    {
        this.shootDir = shootDir;
        if (isCriticalHit)
        {
            sphereDamage *= 2;
            gameObject.GetComponentInChildren<SpriteRenderer>().color = criticalHitSphereColor;
        }
    }

    private void Update()
    {
        float moveSpeed = 30f;
        transform.position += shootDir * moveSpeed * Time.deltaTime;
        
        Vector3 point = camera.WorldToViewportPoint(transform.position);
        if (point.x < 0f || point.x > 1f)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        Enemy enemy = col.GetComponent<Enemy>();

        if (enemy != null && !enemies.Contains(enemy))
        {
            enemy.TakeDamage(sphereDamage, false, false);
            Destroy(gameObject);
        }
        
        enemies.Add(enemy);
    }
}
