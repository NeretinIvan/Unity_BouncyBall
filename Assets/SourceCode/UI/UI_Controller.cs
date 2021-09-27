using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Controller : MonoBehaviour
{
    public enum Screens
    {
        //MainMenu,
        GameOver,
        none
    }
    [SerializeField()] 
    public GameObject[] screens = new GameObject[System.Enum.GetValues(typeof(Screens)).Length];



    private void Awake()
    {
        FindObjectOfType<PauseController>().pauseGame += ShowGameOver;
        FindObjectOfType<PauseController>().resumeGame += ClearAll;
    }

    private void ShowGameOver()
    {
        ShowScreen(Screens.GameOver);
    }

    private void ClearAll()
    {
        ShowScreen(Screens.none);
    }



    public void ShowScreen(Screens screen)
    {
        ShowScreen(screen, true);
    }

    public void ShowScreen(Screens showingScreen, bool hideOtherScreens)
    {
        if (hideOtherScreens)
        {
            foreach(GameObject memberScreen in screens)
            {
                if (memberScreen == null) continue;
                memberScreen.SetActive(false);
            }
        }
        if (showingScreen == Screens.none) return;

        int enumNumber = System.Convert.ToInt32(showingScreen);
        screens[enumNumber].SetActive(true);
    }
}
