using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameLogicController : MonoBehaviour
{
    public event EventHandler<EventArgs> OnGameStarted;

    public event EventHandler<GameOverEventArgs> OnGameOver;
    public class GameOverEventArgs : EventArgs
    {
        public float Score { get; set; }
        public float Time { get; set; }
    }

    public event EventHandler<ScoreUpdateEventArgs> OnScoreUpdate;
    public class ScoreUpdateEventArgs : EventArgs
    {
        public float Score { get; set; }
    }

    public event EventHandler<DifficultyUpdateEventArgs> OnDifficultyUpdate;
    public class DifficultyUpdateEventArgs : EventArgs
    {
        public int Difficulty { get; set; }
    }


    public float Score { get; private set; }
    public int Difficulty { get; private set; }

    [Tooltip("Basic value of score needed to reach new difficulty")]
    [Min(1f)][SerializeField] private float difficultyBaseValue;
    [Tooltip("Each new level of difficulty will require amount of score, multiplied by this number")]
    [Min(1f)][SerializeField] private float difficultyRiseMultiplayer;

    private void Awake()
    {
        OnScoreUpdate += GameLogicController_OnScoreUpdate;
        OnDifficultyUpdate += GameLogicController_OnDifficultyUpdate;
        OnGameStarted += GameLogicController_OnGameStarted;
    }

    private void Start()
    {
        RestartGame();
    }

    private void GameLogicController_OnScoreUpdate(object sender, ScoreUpdateEventArgs e)
    {
        Score = Math.Max(e.Score, 0);
        int newDifficulty = CalculateDifficultyLevel(Score);
        if (newDifficulty != Difficulty)
        {
            OnDifficultyUpdate.Invoke(this, new DifficultyUpdateEventArgs { Difficulty = newDifficulty });
        }     
    }

    private void GameLogicController_OnDifficultyUpdate(object sender, DifficultyUpdateEventArgs e)
    {
        Difficulty = e.Difficulty;
        Debug.Log("New difficulty: " + e.Difficulty);
    }

    private void GameLogicController_OnGameStarted(object sender, EventArgs e)
    {
        SetScore(0);
    }

    private int CalculateDifficultyLevel(float score)
    {
        if (score <= difficultyBaseValue)
        {
            return 1;
        }
        else
        {
            return 1 + CalculateDifficultyLevel((score - difficultyBaseValue) / difficultyRiseMultiplayer);
        }
    }

    public void SetScore(float score)
    {
        OnScoreUpdate.Invoke(this, new ScoreUpdateEventArgs { Score = score });
    }

    public void AddScore(float score)
    {
        OnScoreUpdate.Invoke(this, new ScoreUpdateEventArgs { Score = Score + score });
    }

    public void GameOver()
    {
        OnGameOver.Invoke(this, new GameOverEventArgs { Score = Score, Time = 0 });
    }

    public void RestartGame()
    {
        OnGameStarted.Invoke(this, new EventArgs());
    }
}
