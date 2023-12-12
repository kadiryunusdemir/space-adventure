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
        for (int i = 0; i < 10; i++)
        {
            float angle = Random.Range(0f, 2f * Mathf.PI);
            float distance = Random.Range(0f, radius);

            float x = center.x + distance * Mathf.Cos(angle);
            float y = center.y + distance * Mathf.Sin(angle);
            var randomPoint = new Vector3(x, y, center.z);
            
            var enemy = ObjectPoolManager.Instance.Get(Enums.ObjectPoolType.Enemy).GetComponent<Enemy>();
            enemy.Init(randomPoint, enemySprites[i % 2]);
        }
    }
}
