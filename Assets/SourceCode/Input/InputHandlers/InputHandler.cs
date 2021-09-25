using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InputHandler
{
    public enum InputType
    {
        mouse,
        touch
    }

    public abstract Vector2 GetInputPosition();
    public abstract bool InputStarted();
    public abstract bool InputEnded();
}
