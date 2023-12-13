using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utilities;
using Random = UnityEngine.Random;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private List<Sprite> enemySprites;

    private void Start()
    {
        Vector3 center = this.transform.position;
        float radius = 5f; 
        int maxSpawnAttempts = 100; // Maximum number of attempts to find a non-overlapping position
        float enemyRadius = 1f; // Assuming the enemy has a radius of 1 unit

        for (int i = 0; i < 10; i++)
        {
            Vector3 randomPoint;
            int attempts = 0;
            do
            {
                float angle = Random.Range(0f, 2f * Mathf.PI);
                float distance = Random.Range(0f, radius);

                float x = center.x + distance * Mathf.Cos(angle);
                float y = center.y + distance * Mathf.Sin(angle);
                randomPoint = new Vector3(x, y, center.z);

                // Check for overlaps near this position.
                if(Physics2D.OverlapCircle(randomPoint, enemyRadius) == null) {
                    // We found a non-overlapping spot! Slap it down & call it done.
                    break;
                }
                attempts++;
            } while(attempts < maxSpawnAttempts);

            if(attempts >= maxSpawnAttempts) {
                Debug.LogWarning("Failed to find a non-overlapping position after " + maxSpawnAttempts + " attempts.");
                continue;
            }

            var enemy = ObjectPoolManager.Instance.Get(Enums.ObjectPoolType.Enemy).GetComponent<Enemy>();
            enemy.Init(randomPoint, enemySprites[i % 2]);
        }

    }
}
