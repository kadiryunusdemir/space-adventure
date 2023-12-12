using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;

    public void Init(Vector3 spawnPoint, Sprite sprite)
    {
        spriteRenderer.sprite = sprite;
        transform.position = spawnPoint;
        // transform.parent = spawnPoint;
        // transform.localScale = scale;
    }
}
