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
    [SerializeField()] public GameObject[] screens;

    private void Awake()
    {
        int enumMembers = System.Enum.GetValues(typeof(Screens)).Length;
        screens = new GameObject[enumMembers];
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
                memberScreen.SetActive(false);
            }
        }
        if (showingScreen == Screens.none) return;

        int enumNumber = System.Convert.ToInt32(showingScreen);
        screens[enumNumber].SetActive(true);

    }
}
