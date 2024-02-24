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
        
        Vector3 center = this.transform.position;
        float radius = width < height ? width : height; 
        int maxSpawnAttempts = 100; // Maximum number of attempts to find a non-overlapping position
        float enemyRadius = 2f;

        int dummyEnemyCounterToShowDifferentEnemyTypes = 0;

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
                if(Physics2D.OverlapCircle(randomPoint, enemyRadius) == null) 
                {
                    // await UniTask.Delay(100);
                    if (dummyEnemyCounterToShowDifferentEnemyTypes < 2)
                    {
                        var followEnemy = ObjectPoolManager.Instance.Get(Enums.ObjectPoolType.FollowEnemy).GetComponent<FollowPlayer>();
                        followEnemy.Init(randomPoint);
                    }
                    else if (dummyEnemyCounterToShowDifferentEnemyTypes < 4)
                    {
                        var fastEnemy = ObjectPoolManager.Instance.Get(Enums.ObjectPoolType.FastEnemy).GetComponent<Enemy>();
                        fastEnemy.Init(randomPoint);
                    }
                    else if (dummyEnemyCounterToShowDifferentEnemyTypes < 6)
                    {
                        var largeEnemy = ObjectPoolManager.Instance.Get(Enums.ObjectPoolType.LargeEnemy).GetComponent<Enemy>();
                        largeEnemy.Init(randomPoint);
                    }
                    else 
                    {
                        var enemy = ObjectPoolManager.Instance.Get(Enums.ObjectPoolType.Enemy).GetComponent<Enemy>();
                        enemy.Init(randomPoint);
                    }

                    dummyEnemyCounterToShowDifferentEnemyTypes++;
                    
                    break;
                }
                attempts++;
            } while(attempts < maxSpawnAttempts);

        }
    }
}
