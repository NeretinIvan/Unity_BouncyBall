using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    private Ball ball;
    void Awake()
    {
        ball = FindObjectOfType<Ball>();
    }

    void Update()
    {
        if (transform.position.y < ball.transform.position.y)
        {
            transform.position = new Vector3(transform.position.x, ball.transform.position.y, transform.position.z);
        }
    }
}
