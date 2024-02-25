using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using Utilities;
using Random = UnityEngine.Random;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private float width;
    [SerializeField] private float height;

#if UNITY_EDITOR
    protected void OnDrawGizmos()
    {
        Handles.Label(transform.position, "Enemy Spawner");
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(transform.position, new Vector3(width, height, 0));
    }
#endif

    public async UniTask CreateAsteroidShower(LevelData levelData)
    {
        Debug.Log("TODO: level creation with level data, for example wave: " + levelData.Waves[0].WaveDensityPercentage);
        //TODO: remove this dummy asteroid shower, create challenging shower important to game's playability  
        foreach (var wave in levelData.Waves)
        {
            Vector3 center = this.transform.position;
            float radius = width < height ? width : height;
            int maxSpawnAttempts = 100; // Maximum number of attempts to find a non-overlapping position
            float enemyRadius = 2f;

            //int dummyEnemyCounterToShowDifferentEnemyTypes = 0;

            int[] enemyTypeActive = wave.enemyTypeActive;
            int[] enemyTypeCount = wave.enemyTypeCount;
            int[] createdEnemyTypeCount = { 0, 0, 0, 0 };

            for (int i = 0; i < 10; i++)
            {
                int attempts = 0;
                do
                {
                    float angle = Random.Range(0f, 2f * Mathf.PI);
                    float distance = Random.Range(0f, radius);

                    float x = center.x + distance * Mathf.Cos(angle);
                    float y = center.y + distance * Mathf.Sin(angle);
                    var randomPoint = new Vector3(x, y, center.z);
                    // Check for overlaps near this position.
                    if (Physics2D.OverlapCircle(randomPoint, enemyRadius) == null)
                    {
                        // await UniTask.Delay(100);
                        if (enemyTypeActive[3] == 1 && createdEnemyTypeCount[3] < enemyTypeCount[3])
                        {
                            var followEnemy = ObjectPoolManager.Instance.Get(Enums.ObjectPoolType.FollowEnemy).GetComponent<FollowPlayer>();
                            followEnemy.Init(randomPoint);
                            createdEnemyTypeCount[3]++;
                        }
                        else if (enemyTypeActive[2] == 1 && createdEnemyTypeCount[2] < enemyTypeCount[2])
                        {
                            var fastEnemy = ObjectPoolManager.Instance.Get(Enums.ObjectPoolType.FastEnemy).GetComponent<Enemy>();
                            fastEnemy.Init(randomPoint);
                            createdEnemyTypeCount[2]++;
                        }
                        else if (enemyTypeActive[1] == 1 && createdEnemyTypeCount[1] < enemyTypeCount[1])
                        {
                            var largeEnemy = ObjectPoolManager.Instance.Get(Enums.ObjectPoolType.LargeEnemy).GetComponent<Enemy>();
                            largeEnemy.Init(randomPoint);
                            createdEnemyTypeCount[1]++;
                        }

                        else if (enemyTypeActive[0] == 1 && createdEnemyTypeCount[0] < enemyTypeCount[0])
                        {
                            var enemy = ObjectPoolManager.Instance.Get(Enums.ObjectPoolType.Enemy).GetComponent<Enemy>();
                            enemy.Init(randomPoint);
                            createdEnemyTypeCount[0]++;
                        }

                        //dummyEnemyCounterToShowDifferentEnemyTypes++;

                        break;
                    }
                    attempts++;
                    //await UniTask.WaitForSeconds(5);
                } while (attempts < maxSpawnAttempts);

            }
        }

    }
    //public async UniTask CreateAsteroidShowerInLineFormat(LevelData levelData)
    //{
    //    foreach (var wave in levelData.Waves)
    //    {
    //        Vector3 center = this.transform.position;
    //        float enemyDiameter = 2f; // Assuming each enemy takes up a square area
    //        float padding = 0.5f; // Space between enemies

    //        // Calculate how many enemies can fit in a row and how many rows are needed
    //        int enemiesPerRow = Mathf.FloorToInt((width + padding) / (enemyDiameter + padding));
    //        int numberOfRows = Mathf.FloorToInt((height + padding) / (enemyDiameter + padding));

    //        Vector3 startPosition = center - new Vector3(width / 2, height / 2, 0) + new Vector3(enemyDiameter / 2, enemyDiameter / 2, 0);

    //        int[] enemyTypeActive = wave.enemyTypeActive;

    //        int[] enemyTypeCount = wave.enemyTypeCount;
    //        int totalCreateEnemyCount = enemyTypeCount[0] + enemyTypeCount[1] + enemyTypeCount[2] + enemyTypeCount[3];

    //        int[] createdEnemyTypeCount = { 0, 0, 0, 0 };

    //        for (int row = 0; row < numberOfRows; row++)
    //        {
    //            for (int col = 0; col < enemiesPerRow; col++)
    //            {
    //                int totalCreatedEnemyCount = createdEnemyTypeCount[0] + createdEnemyTypeCount[1] + createdEnemyTypeCount[2] + createdEnemyTypeCount[3];
    //                if (totalCreateEnemyCount <= totalCreatedEnemyCount)
    //                {
    //                    continue; // Stop spawning if all enemies have been spawned
    //                }

    //                Vector3 position = startPosition + new Vector3(col * (enemyDiameter + padding), row * (enemyDiameter + padding), 0);

    //                // Reuse your existing if-else structure for spawning enemies, with position adjustments
    //                if (enemyTypeActive[3] == 1 && createdEnemyTypeCount[3] < enemyTypeCount[3])
    //                {
    //                    var followEnemy = ObjectPoolManager.Instance.Get(Enums.ObjectPoolType.FollowEnemy).GetComponent<FollowPlayer>();
    //                    followEnemy.Init(position);
    //                    createdEnemyTypeCount[3]++;
    //                }
    //                else if (enemyTypeActive[2] == 1 && createdEnemyTypeCount[2] < enemyTypeCount[2])
    //                {
    //                    var fastEnemy = ObjectPoolManager.Instance.Get(Enums.ObjectPoolType.FastEnemy).GetComponent<Enemy>();
    //                    fastEnemy.Init(position);
    //                    createdEnemyTypeCount[2]++;
    //                }
    //                else if (enemyTypeActive[1] == 1 && createdEnemyTypeCount[1] < enemyTypeCount[1])
    //                {
    //                    var largeEnemy = ObjectPoolManager.Instance.Get(Enums.ObjectPoolType.LargeEnemy).GetComponent<Enemy>();
    //                    largeEnemy.Init(position);
    //                    createdEnemyTypeCount[1]++;
    //                }
    //                else if (enemyTypeActive[0] == 1 && createdEnemyTypeCount[0] < enemyTypeCount[0])
    //                {
    //                    var enemy = ObjectPoolManager.Instance.Get(Enums.ObjectPoolType.Enemy).GetComponent<Enemy>();
    //                    enemy.Init(position);
    //                    createdEnemyTypeCount[0]++;
    //                }
    //            }
    //        }
    //    }
    //}
}
