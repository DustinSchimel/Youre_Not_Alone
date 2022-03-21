using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{

    [SerializeField] public float walkSpeed;
    private bool movingRight = true;
    public Transform groundDetection;
    public Transform wallDetection;

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector2.right * walkSpeed * Time.deltaTime);

        
        RaycastHit2D groundInfo = Physics2D.Raycast(groundDetection.position, Vector2.down, 2.0f);
        RaycastHit2D wallInfo = Physics2D.Raycast(wallDetection.position, Vector2.right, 2.0f);
        if (!groundInfo.collider || wallInfo.collider)
        {
            if (movingRight)
            {
                transform.eulerAngles = new Vector3(0, -180, 0);
                movingRight = false;
            }
            else
            {
                transform.eulerAngles = new Vector3(0, 0, 0);
                movingRight = true;
            }
        }
    }
}
