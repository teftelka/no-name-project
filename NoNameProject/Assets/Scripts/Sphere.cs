using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sphere : MonoBehaviour
{
    private Vector3 shootDir;
    [SerializeField] private int sphereDamage = 100;
    List<Enemy> enemies = new List<Enemy>();
    private ScoreManager scoreManager;

    private void Awake()
    {
        scoreManager = FindObjectOfType<ScoreManager>();
    }

    public void Setup(Vector3 shootDir)
    {
        this.shootDir = shootDir;
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
