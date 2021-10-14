using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ColorPalette
{
    public Color ballTraceColor;
    public Color glowingObstaclesColor;
    public Color lavaColor;
    public Color wallsColor;
    public Color backWallColor;
    public IntRange appearsInDifficulty;
}

[System.Serializable]
public struct IntRange
{
    public int from;
    public int to;
}