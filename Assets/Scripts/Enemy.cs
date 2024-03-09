using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utilities;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private IntSO scoreSO;
    [SerializeField] private IntSO healthSO;
    [SerializeField] private IntSO meteorSO;
    [SerializeField] private int enemyHealth;
    private Transform playerTarget; // To store the player's transform
    private Rigidbody2D rb2D;
    private int actualHealth;
    public void Init(Vector3 spawnPoint)
    {
        this.transform.position = spawnPoint;
        actualHealth = enemyHealth;
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
            SoundManager.Instance.PlaySound(Enums.Sound.EnemyDie, transform.position);

            healthSO.DecreaseInt(enemyHealth);
            ObjectPoolManager.Instance.ReturnToPool(this.gameObject);
        }
        else if (other.CompareTag("Bullet"))
        {
            SoundManager.Instance.PlaySound(Enums.Sound.EnemyHit, transform.position);

            actualHealth -= 1;
            if (actualHealth <= 0)
            {
                SoundManager.Instance.PlaySound(Enums.Sound.EnemyDie, transform.position);

                // UIManager.Instance.DisplayMessage(transform.position, enemyHealth.ToString());
                scoreSO.IncreaseInt(enemyHealth);
                meteorSO.IncreaseInt(1);
                ObjectPoolManager.Instance.ReturnToPool(this.gameObject);
            }
            
            GameObject bullet = other.gameObject;
            ObjectPoolManager.Instance.ReturnToPool(bullet);
        }
        else if (other.CompareTag("Enemy"))
        {
            Debug.Log("Enemies are collided. Prevent?");
        }
    }
}
