using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utilities;

public class Bullet : MonoBehaviour
{
    float maxSpeed = 8f;

    private void Update()
    {
        // movement
        Vector3 pos = transform.position;

        Vector3 velocity = new Vector3(0, maxSpeed * Time.deltaTime, 0);

        pos += transform.rotation * velocity;

        transform.position = pos;
    }
}
