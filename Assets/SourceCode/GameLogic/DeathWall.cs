using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathWall : MonoBehaviour
{
    [Min(0)] [SerializeField()] private float maxDistanceFromCamera = 1;
    [Tooltip("Time, in which wall stands still before move on, in seconds")]
    [Min(0)] [SerializeField()] private float cooldownTime = 2;
    [Min(0)] [SerializeField()] private float speed = 2;

    private float cooldownRemaining;

    private float cooldownMultiplayer = 1;
    private float speedMultiplayer = 1;

    private void Awake()
    {
        Vector3 ballPosition = GameObject.FindObjectOfType<Ball>().transform.position;
        transform.position = new Vector3(ballPosition.x, transform.position.y, transform.position.z);
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent(out Ball ball))
        {
            ball.BallFailed();
        }
    }

    void Update()
    {
        cooldownRemaining = Mathf.Max(cooldownRemaining - Time.deltaTime, 0);
        TryMove();
        SyncWithCamera();
    }

    private void ResetCooldown()
    {
        cooldownRemaining = cooldownTime;
    }

    private void SyncWithCamera()
    {
        if (transform.position.y + maxDistanceFromCamera < Camera.main.transform.position.y)
        {
            transform.position = new Vector3(
                transform.position.x,
                Camera.main.transform.position.y - maxDistanceFromCamera,
                transform.position.z);
            ResetCooldown();
        }
    }

    private void TryMove()
    {
        if (cooldownRemaining <= 0)
        {
            transform.position = new Vector3(
            transform.position.x,
            transform.position.y + speed * Time.deltaTime,
            transform.position.z);
        }       
    }
}
