using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Utilities;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private IntSO scoreSO;
    [SerializeField] private IntSO healthSO;
    [SerializeField] private IntSO meteorSO;
    [SerializeField] private int enemyHealth;
    [SerializeField] private ParticleSystem explosionParticle;
    private Transform playerTarget; // To store the player's transform
    private Rigidbody2D rb2D;
    private int actualHealth;
    private Vector3 shakeStr;
    private float shakeTime;
    private bool isDestructionInitiated = false;

    public void Init(Vector3 spawnPoint)
    {
        this.transform.position = spawnPoint;
        actualHealth = enemyHealth;
        shakeStr = Vector3.one / 10;
        shakeTime = 0.2f;
        isDestructionInitiated = false;
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
        if (other.CompareTag("Enemy"))
        {
            Debug.Log("Enemies are collided. Prevent?");
            return;
        }

        if (other.CompareTag("Bullet"))
        {
            // SoundManager.Instance.PlaySound(Enums.Sound.EnemyHit, transform.position);

            actualHealth -= 1;
            
            transform.DOComplete();
            transform.DOShakeRotation(shakeTime, shakeStr);
            transform.DOShakePosition(shakeTime, shakeStr);
            
            GameObject bullet = other.gameObject;
            ObjectPoolManager.Instance.ReturnToPool(bullet);
        }
        
        if (!isDestructionInitiated && (other.CompareTag("Player") || actualHealth <= 0))
        {
            isDestructionInitiated = true;
            if (other.CompareTag("Player"))
            {
                other.transform.DOComplete();
                other.transform.DOShakeRotation(shakeTime, shakeStr);
                other.transform.DOShakePosition(shakeTime, shakeStr);
                healthSO.DecreaseInt(enemyHealth);
                scoreSO.DecreaseInt(enemyHealth);
            }
            
            explosionParticle.Play();
            // TODO: bu ses game managerdan tetiklenenleri eziyor
            // SoundManager.Instance.PlaySound(Enums.Sound.EnemyDie, transform.position);

            // UIManager.Instance.DisplayMessage(transform.position, enemyHealth.ToString());
            scoreSO.IncreaseInt(enemyHealth);

            meteorSO.IncreaseInt(1);

            transform.DOComplete();
            transform.DOShakeRotation(shakeTime, shakeStr);
            transform.DOShakePosition(shakeTime, shakeStr).OnComplete(() =>
            {
                ObjectPoolManager.Instance.ReturnToPool(this.gameObject);
            });

        }
    }
}
