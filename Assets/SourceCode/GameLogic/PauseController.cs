using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseController : MonoBehaviour
{
    public event PauseGameDelegate pauseGame;
    public event ResumeGameDelegate resumeGame;
    public delegate void PauseGameDelegate();
    public delegate void ResumeGameDelegate();
    public bool GamePaused { get; private set; } = false;

    private void Awake()
    {
        pauseGame += PauseGame;
        resumeGame += ResumeGame;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (GamePaused)
            {
                resumeGame.Invoke();
            }
            else
            {
                pauseGame.Invoke();
            }
        }
    }

    private void PauseGame()
    {
        Time.timeScale = 0;
        GamePaused = true;
    }

    private void ResumeGame()
    {
        Time.timeScale = 1;
        GamePaused = false;
    }
}
