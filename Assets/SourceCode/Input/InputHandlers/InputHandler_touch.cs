using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandler_touch : InputHandler
{
    public override Vector2 GetInputPosition()
    {
        return Input.GetTouch(0).position;
    }

    public override bool InputEnded()
    {
        return Input.GetTouch(0).phase == TouchPhase.Ended;
    }

    public override bool InputStarted()
    {
        return Input.GetTouch(0).phase == TouchPhase.Began;
    }
}
