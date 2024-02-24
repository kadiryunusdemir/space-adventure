using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utilities;

public class FollowPlayer : MonoBehaviour
{
    // Start is called before the first frame update
    public float speed = 5f;
    private Transform playerTransform;
    private bool isFollowingPlayer = true;
    private bool startFalling = false;
    
    public void Init(Vector3 spawnPoint)
    {
        this.transform.position = spawnPoint;
        // transform.parent = spawnPoint;
        // transform.localScale = scale;
    }
    
    void Start()
    {
        // Find the player ship by tag.
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
        }
        else
        {
            Debug.Log("Player object not found. Ensure your player has the 'Player' tag.");
            isFollowingPlayer = false;
        }
    }

    void Update()
    {
        if (isFollowingPlayer && playerTransform != null && !startFalling)
        {
            Vector3 direction = (playerTransform.position - transform.position).normalized;
            transform.position += direction * speed * Time.deltaTime;
        }
        else if (startFalling)
        {
            // Make the meteor fall straight down
            transform.position += Vector3.down * speed * Time.deltaTime;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            isFollowingPlayer = false; // Stop following the player
            startFalling = true; // Start falling down

            // Schedule return to pool after a delay to allow for falling animation
            StartCoroutine(ReturnToPoolAfterDelay(2f)); // Adjust delay as needed
        }
    }
    IEnumerator ReturnToPoolAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        if (ObjectPoolManager.Instance != null)
        {
            ObjectPoolManager.Instance.ReturnToPool(gameObject);
        }
        else
        {
            Debug.Log("ObjectPoolManager.Instance is null. Check if ObjectPoolManager is initialized correctly.");
        }
    }
}
