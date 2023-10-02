using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class CircleDrawer : MonoBehaviour
{
    [SerializeField] private Sprite aimImage;
    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
    }

    public void DrawCircle(Vector3 position)
    {
        transform.position = position;
        spriteRenderer.sprite = aimImage;
    }
    
    public void RemoveCircle()
    {
        spriteRenderer.sprite = null;
    }

}
