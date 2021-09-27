using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLogicController : MonoBehaviour
{
    private UI_Controller ui_controller;

    private void Awake()
    {
        ui_controller = FindObjectOfType<UI_Controller>();      
    }

    private void Start()
    {
        ui_controller.ShowScreen(UI_Controller.Screens.none);
    }
}
