using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ColorPalette
{
    [ColorUsage(true, true)] public Color ballTraceColor;
    [ColorUsage(true, true)] public Color glowingObstaclesColor;
    [ColorUsage(true, true)] public Color lavaColor;
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