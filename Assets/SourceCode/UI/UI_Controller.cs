using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class UI_Controller : MonoBehaviour
{
    public enum ScreenType
    {
        MainMenu,
        GameOver,
        none,
        ScoreLabel
    }

    [System.Serializable]
    public class Screen
    {
        public ScreenType type;
        public GameObject rootObject;
    }

    [Min(0)][SerializeField] private int maxDisplayedScore;
    [SerializeField] private Text scoreTextLabel;
    [SerializeField] private Text gameOverScoreLabel;
    public List<Screen> screens;

    private void Awake()
    {
        GameLogicController gameLogicController = FindObjectOfType<GameLogicController>();
        gameLogicController.OnScoreUpdate += GameLogicController_OnScoreUpdate;
        gameLogicController.OnGameOver += GameLogicController_OnGameOver;
        gameLogicController.OnGameStarted += GameLogicController_OnGameStarted;
    }

    public void ShowScreen(ScreenType screen)
    {
        ShowScreen(screen, true);
    }

    public void ShowScreen(ScreenType showingScreen, bool hideOtherScreens)
    {
        foreach (Screen memberScreen in screens)
        {
            if (memberScreen == null) continue;
            if (memberScreen.rootObject == null) continue;

            if(memberScreen.type == showingScreen)
            {
                memberScreen.rootObject.SetActive(true);
            }
            else if (hideOtherScreens)
            {
                memberScreen.rootObject.SetActive(false);
            }          
        }
    }

    private void GameLogicController_OnScoreUpdate(object sender, GameLogicController.ScoreUpdateEventArgs e)
    {
        scoreTextLabel.text = Mathf.Min(e.Score, maxDisplayedScore).ToString();
    }

    private void GameLogicController_OnGameOver(object sender, GameLogicController.GameOverEventArgs e)
    {
        ShowScreen(ScreenType.GameOver);
        gameOverScoreLabel.text = e.Score.ToString();
        FindObjectOfType<PauseController>().PauseGame();
    }

    private void GameLogicController_OnGameStarted(object sender, System.EventArgs e)
    {
        ShowScreen(ScreenType.none);
        ShowScreen(ScreenType.ScoreLabel);
        FindObjectOfType<PauseController>().ResumeGame();
    }
}
