using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandler_mouse : InputHandler
{
    public override Vector2 GetInputPosition()
    {
        return Input.mousePosition;
    }

    public override bool InputEnded()
    {
        return Input.GetMouseButtonUp(0);
    }

    public override bool InputStarted()
    {
        return Input.GetMouseButtonDown(0);
    }
}
