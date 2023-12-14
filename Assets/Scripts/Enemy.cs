using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utilities;

public class Enemy : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private float speed = 3;
    [SerializeField] private IntSO scoreSO;
    [SerializeField] private IntSO healthSO;

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
        if (other.CompareTag("Player"))
        {
            healthSO.DecreaseInt(1);
                
            ObjectPoolManager.Instance.ReturnToPool(this.gameObject);

            if (healthSO.Number == 0)
            {
                Debug.Log("GAME OVER");
                //TODO: game end, this logic can be transfer to game manager
            }
        }
        else if (other.CompareTag("Bullet"))
        {
            scoreSO.IncreaseInt(1);

            ObjectPoolManager.Instance.ReturnToPool(this.gameObject);
            
            GameObject bullet = other.gameObject;
            ObjectPoolManager.Instance.ReturnToPool(bullet);
        }
        else if (other.CompareTag("Enemy"))
        {
            Debug.Log("this not be happen");
        }
    }
}
