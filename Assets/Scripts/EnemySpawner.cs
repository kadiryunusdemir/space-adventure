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
    [SerializeField] private Camera mainCamera;
    private int totalSpawns;
    private float spawnDelay;

#if UNITY_EDITOR
    protected void OnDrawGizmos()
    {
        Handles.Label(transform.position, "Enemy Spawner");
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(transform.position, new Vector3(width, height, 0));
    }
#endif
    public async UniTask CreateAsteroidShower2(LevelData levelData)
    {
        float screenWidthInWorldUnits = mainCamera.ScreenToWorldPoint(new Vector3(Screen.width, 0, 0)).x - mainCamera.ScreenToWorldPoint(new Vector3(0, 0, 0)).x;
        totalSpawns = Mathf.CeilToInt(screenWidthInWorldUnits);
        Vector3 leftEdge = mainCamera.ScreenToWorldPoint(new Vector3(0, 0, mainCamera.nearClipPlane));
        Vector3 rightEdge = mainCamera.ScreenToWorldPoint(new Vector3(Screen.width, 0, 0));
        float spawnPointInterval = (float)((rightEdge.x - leftEdge.x) / 18.0);
        // Start the spawning process
        foreach (var wave in levelData.Waves)
        {

            int[] spawnDelay = wave.delayAmount;
            int[] enemyType = wave.enemyType;
            int[] enemySpawnPoint = wave.enemySpawnPoint;
            int enemyCount = wave.enemyCount;
            int createdEnemyCount = 0;

            while(createdEnemyCount < enemyCount)
            {
                //Debug.Log("Creating enemy: " + createdEnemyCount + "for enemy count: " + enemyCount);
                int randomSpawnPointUnit = enemySpawnPoint[createdEnemyCount];

                Vector3 spawnPoint = new Vector3(leftEdge.x + (randomSpawnPointUnit * spawnPointInterval),(float) 7, 0);

                // Instantiate the asteroid at the calculated position

                SpawnAsteroid(spawnPoint, enemyType[createdEnemyCount]);
                await UniTask.Delay(spawnDelay[createdEnemyCount]);
                createdEnemyCount++;

            }

            Debug.Log("Created Meteor Count: " + createdEnemyCount + " Ending spawner");
        }
    }
    private void SpawnAsteroid(Vector3 position, int enemyType)
    {
        // Instantiate the asteroid at the given position
        if (enemyType == 3)
        {
            var followEnemy = ObjectPoolManager.Instance.Get(Enums.ObjectPoolType.FollowEnemy).GetComponent<FollowPlayer>();
            followEnemy.Init(position);
        }
        else if (enemyType == 2)
        {
            var fastEnemy = ObjectPoolManager.Instance.Get(Enums.ObjectPoolType.FastEnemy).GetComponent<Enemy>();
            fastEnemy.Init(position);
        }
        else if (enemyType == 1)
        {
            var largeEnemy = ObjectPoolManager.Instance.Get(Enums.ObjectPoolType.LargeEnemy).GetComponent<Enemy>();
            largeEnemy.Init(position);
        }

        else if (enemyType == 0)
        {
            var enemy = ObjectPoolManager.Instance.Get(Enums.ObjectPoolType.Enemy).GetComponent<Enemy>();
            enemy.Init(position);
        }
    }
    //public async UniTask CreateAsteroidShower(LevelData levelData)
    //{
    //    Debug.Log("TODO: level creation with level data, for example wave: " + levelData.Waves[0].WaveDensityPercentage);
    //    //TODO: remove this dummy asteroid shower, create challenging shower important to game's playability  
    //    foreach (var wave in levelData.Waves)
    //    {
    //        Vector3 center = this.transform.position;
    //        float radius = width < height ? width : height;
    //        int maxSpawnAttempts = 100; // Maximum number of attempts to find a non-overlapping position
    //        float enemyRadius = 2f;

    //        //int dummyEnemyCounterToShowDifferentEnemyTypes = 0;

    //        int[] enemyTypeActive = wave.enemyTypeActive;
    //        int[] enemyTypeCount = wave.enemyTypeCount;
    //        int[] createdEnemyTypeCount = { 0, 0, 0, 0 };

    //        for (int i = 0; i < 10; i++)
    //        {
    //            int attempts = 0;
    //            do
    //            {
    //                float angle = Random.Range(0f, 2f * Mathf.PI);
    //                float distance = Random.Range(0f, radius);

    //                float x = center.x + distance * Mathf.Cos(angle);
    //                float y = center.y + distance * Mathf.Sin(angle);
    //                var randomPoint = new Vector3(x, y, center.z);
    //                // Check for overlaps near this position.
    //                if (Physics2D.OverlapCircle(randomPoint, enemyRadius) == null)
    //                {
    //                    // await UniTask.Delay(100);
    //                    if (enemyTypeActive[3] == 1 && createdEnemyTypeCount[3] < enemyTypeCount[3])
    //                    {
    //                        var followEnemy = ObjectPoolManager.Instance.Get(Enums.ObjectPoolType.FollowEnemy).GetComponent<FollowPlayer>();
    //                        followEnemy.Init(randomPoint);
    //                        createdEnemyTypeCount[3]++;
    //                    }
    //                    else if (enemyTypeActive[2] == 1 && createdEnemyTypeCount[2] < enemyTypeCount[2])
    //                    {
    //                        var fastEnemy = ObjectPoolManager.Instance.Get(Enums.ObjectPoolType.FastEnemy).GetComponent<Enemy>();
    //                        fastEnemy.Init(randomPoint);
    //                        createdEnemyTypeCount[2]++;
    //                    }
    //                    else if (enemyTypeActive[1] == 1 && createdEnemyTypeCount[1] < enemyTypeCount[1])
    //                    {
    //                        var largeEnemy = ObjectPoolManager.Instance.Get(Enums.ObjectPoolType.LargeEnemy).GetComponent<Enemy>();
    //                        largeEnemy.Init(randomPoint);
    //                        createdEnemyTypeCount[1]++;
    //                    }

    //                    else if (enemyTypeActive[0] == 1 && createdEnemyTypeCount[0] < enemyTypeCount[0])
    //                    {
    //                        var enemy = ObjectPoolManager.Instance.Get(Enums.ObjectPoolType.Enemy).GetComponent<Enemy>();
    //                        enemy.Init(randomPoint);
    //                        createdEnemyTypeCount[0]++;
    //                    }

    //                    //dummyEnemyCounterToShowDifferentEnemyTypes++;

    //                    break;
    //                }
    //                attempts++;
    //                //await UniTask.WaitForSeconds(5);
    //            } while (attempts < maxSpawnAttempts);

    //        }
    //    }

    //}
}
