using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{   
    float maxSpeedy = 5f;
    float maxSpeedx = 5f;
    float shipBoundaryRadius = 0.5f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
       

        Vector3 pos = transform.position;

        pos.y += Input.GetAxis("Vertical")*maxSpeedy * Time.deltaTime;
        pos.x += Input.GetAxis("Horizontal") * maxSpeedx * Time.deltaTime;

        //vertical bounds
        if (pos.y + shipBoundaryRadius > Camera.main.orthographicSize)
        {
            pos.y = Camera.main.orthographicSize - shipBoundaryRadius;
        }
        if (pos.y - shipBoundaryRadius < -Camera.main.orthographicSize)
        {
            pos.y = -Camera.main.orthographicSize + shipBoundaryRadius;
        }

        //calculate the orthographic width based on the screen ratio
        float screenRatio = (float)Screen.width / (float)Screen.height;
        float widthOrtho = Camera.main.orthographicSize * screenRatio;

        //horizontal bounds
        if (pos.x + shipBoundaryRadius > widthOrtho)
        {
            pos.x = widthOrtho - shipBoundaryRadius;
        }
        if (pos.x - shipBoundaryRadius < -widthOrtho)
        {
            pos.x = -widthOrtho + shipBoundaryRadius;
        }

        transform.position = pos;
        
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        Debug.LogError(other.gameObject.layer);
    }
}
