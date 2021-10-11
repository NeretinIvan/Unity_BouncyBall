using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestartButton : MonoBehaviour
{
    GameLogicController gameLogicController;
    private void Awake()
    {
        gameLogicController = FindObjectOfType<GameLogicController>();
    }

    public void OnClick()
    {
        gameLogicController.RestartGame();
    }
}
