using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformItems : MonoBehaviour
{
    [SerializeField] private GameObject coin;
    [SerializeField] private GameObject heart;
    [SerializeField] private Transform coinSpawnPoint;
    [SerializeField] private Sprite block;
    private int itemsCount = 1;
    private SpriteRenderer _spriteRenderer;

    private void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Player") && col is BoxCollider2D && itemsCount != 0)
        {
            var newCoin = Instantiate(coin, coinSpawnPoint.position, Quaternion.identity, coinSpawnPoint);
            //var newCoin = Instantiate(heart, coinSpawnPoint.position, Quaternion.identity, coinSpawnPoint);

            ChangeSprite();
            itemsCount--;
        }
    }

    private void ChangeSprite()
    {
        _spriteRenderer.sprite = block;
    }
}
