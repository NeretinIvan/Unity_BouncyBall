using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PauseController : MonoBehaviour
{
    public event EventHandler<EventArgs> OnPauseGame;
    public event EventHandler<EventArgs> OnResumeGame;

    public bool GamePaused { get; private set; } = false;

    private void Awake()
    {
        OnPauseGame += PauseController_pauseGame;
        OnResumeGame += PauseController_resumeGame;
    }

    private void PauseController_resumeGame(object sender, EventArgs e)
    {
        Time.timeScale = 1;
        GamePaused = false;
    }

    private void PauseController_pauseGame(object sender, EventArgs e)
    {
        Time.timeScale = 0;
        GamePaused = true;
    }

    public void PauseGame()
    {
        OnPauseGame.Invoke(this, new EventArgs());
    }

    public void ResumeGame()
    {
        OnResumeGame.Invoke(this, new EventArgs());
    }
}
