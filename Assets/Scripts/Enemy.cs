using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private float speed = 5;

    private Rigidbody2D rb2D;
    
    public void Init(Vector3 spawnPoint, Sprite sprite)
    {
        spriteRenderer.sprite = sprite;
        transform.position = spawnPoint;
        // transform.parent = spawnPoint;
        // transform.localScale = scale;
    }

    private void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        rb2D.MovePosition(rb2D.position + Vector2.down * speed * Time.fixedDeltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("BOOM");
        }
        else if (other.gameObject.CompareTag("Bullet"))
        {
            Debug.Log("Destroy asteroid");
        }
        else if (other.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("this not be happen");
        }
    }
}
