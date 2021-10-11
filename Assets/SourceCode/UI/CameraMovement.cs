using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    private Ball ball;
    private GameLogicController gameLogicController;
    private Vector3 startingPosition;

    void Awake()
    {
        startingPosition = transform.position;
        ball = FindObjectOfType<Ball>();
        gameLogicController = FindObjectOfType<GameLogicController>();
        gameLogicController.OnGameStarted += GameLogicController_OnGameStarted;
    }

    void Update()
    {
        if (transform.position.y < ball.transform.position.y)
        {
            transform.position = new Vector3(transform.position.x, ball.transform.position.y, transform.position.z);
        }
    }

    private void GameLogicController_OnGameStarted(object sender, System.EventArgs e)
    {
        transform.position = startingPosition;
    }
}
