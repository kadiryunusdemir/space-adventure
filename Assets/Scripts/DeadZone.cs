using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using Utilities;

public class EnemyDeadZone : MonoBehaviour
{
    [SerializeField] private IntSO healthSO;
    [SerializeField] private IntSO meteorSO;
    [SerializeField] private BoxCollider2D boxCollider2D;

#if UNITY_EDITOR
    protected void OnDrawGizmos()
    {
        Handles.Label(boxCollider2D.bounds.center, "Dead Zone");
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(boxCollider2D.bounds.center, boxCollider2D.bounds.size);
    }
#endif
    
    private async void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            var enemy = other.gameObject.GetComponent<Enemy>();
            // TODO: decrease the health by the enemy health
            healthSO.DecreaseInt(1);
            meteorSO.IncreaseInt(1);
        }

        ObjectPoolManager.Instance.ReturnToPool(other.gameObject);
    }
}
