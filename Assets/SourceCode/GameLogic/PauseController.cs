using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PauseController : MonoBehaviour
{
    public event EventHandler<EventArgs> OnPauseGame;
    public event EventHandler<EventArgs> OnResumeGame;
    public event EventHandler<SlowTimeEventArgs> OnSlowTime;
    public event EventHandler<EventArgs> OnRemoveSlowTime;

    public class SlowTimeEventArgs : EventArgs
    {
        private float slowTimeMultiplayer = 0.5f;
        public float SlowTimeMultiplayer
        {
            get
            {
                return slowTimeMultiplayer;
            }
            set
            {
                slowTimeMultiplayer = Mathf.Clamp01(value);
            }
        }
    }

    public bool GamePaused { get; private set; } = false;
    public const float DEFAULT_TIMESCALE = 1;

    private void Awake()
    {
        OnPauseGame += PauseController_pauseGame;
        OnResumeGame += PauseController_resumeGame;
        OnSlowTime += PauseController_OnSlowTime;
        OnRemoveSlowTime += PauseController_OnRemoveSlowTime;
        Time.timeScale = DEFAULT_TIMESCALE;
    }

    private void PauseController_OnRemoveSlowTime(object sender, EventArgs e)
    {
        Time.timeScale = DEFAULT_TIMESCALE;
    }

    private void PauseController_OnSlowTime(object sender, SlowTimeEventArgs e)
    {
        Time.timeScale = e.SlowTimeMultiplayer;
    }

    private void PauseController_resumeGame(object sender, EventArgs e)
    {
        Time.timeScale = DEFAULT_TIMESCALE;
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

    public void SlowTime(float multiplayer)
    {
        OnSlowTime.Invoke(this, new SlowTimeEventArgs { SlowTimeMultiplayer = multiplayer });
    }

    public void RemoveSlowTime()
    {
        OnRemoveSlowTime.Invoke(this, new EventArgs());
    }
}
