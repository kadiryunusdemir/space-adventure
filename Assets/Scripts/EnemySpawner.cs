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

    // private Vector3 currentEnemySpawnPoint;
    // private int currentEnemyType;
    // private int currentCreatedEnemyCount;

#if UNITY_EDITOR
    protected void OnDrawGizmos()
    {
        Handles.Label(transform.position, "Enemy Spawner");
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(transform.position, new Vector3(width, height, 0));
    }
#endif
    public void CreateAsteroidShower(LevelData levelData)
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
            float prevTotalDelay = 0;

            while(createdEnemyCount < enemyCount)
            {
                //Debug.Log("Creating enemy: " + createdEnemyCount + "for enemy count: " + enemyCount);
                int randomSpawnPointUnit = enemySpawnPoint[createdEnemyCount];

                Vector3 spawnPoint = new Vector3(leftEdge.x + (randomSpawnPointUnit * spawnPointInterval),(float) 7, 0);

                // Instantiate the asteroid at the calculated position

                var currentEnemyType = enemyType[createdEnemyCount];
                prevTotalDelay += spawnDelay[createdEnemyCount] / 1000f;
                createdEnemyCount++;
                StartCoroutine(SpawnAsteroid(currentEnemyType, spawnPoint, prevTotalDelay));
            }

            Debug.Log("Created Meteor Count: " + createdEnemyCount + " Ending spawner");
        }
    }
    
    IEnumerator SpawnAsteroid(int currentEnemyType, Vector3 currentEnemySpawnPoint, float delay)
    {
        yield return new WaitForSeconds(delay);

        // Instantiate the asteroid at the given position
        if (currentEnemyType == 3)
        {
            var followEnemy = ObjectPoolManager.Instance.Get(Enums.ObjectPoolType.FollowEnemy).GetComponent<FollowPlayer>();
            followEnemy.Init(currentEnemySpawnPoint);
        }
        else if (currentEnemyType == 2)
        {
            var fastEnemy = ObjectPoolManager.Instance.Get(Enums.ObjectPoolType.FastEnemy).GetComponent<Enemy>();
            fastEnemy.Init(currentEnemySpawnPoint);
        }
        else if (currentEnemyType == 1)
        {
            var largeEnemy = ObjectPoolManager.Instance.Get(Enums.ObjectPoolType.LargeEnemy).GetComponent<Enemy>();
            largeEnemy.Init(currentEnemySpawnPoint);
        }

        else if (currentEnemyType == 0)
        {
            var enemy = ObjectPoolManager.Instance.Get(Enums.ObjectPoolType.Enemy).GetComponent<Enemy>();
            enemy.Init(currentEnemySpawnPoint);
        }
    }
    
    public void StopSpawner()
    {
        StopAllCoroutines();
    }
}
