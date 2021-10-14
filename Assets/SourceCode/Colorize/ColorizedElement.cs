using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorizedElement : MonoBehaviour
{
    public ColorizedElementType type;
    [System.Serializable]
    public enum ColorizedElementType
    {
        BallTrace,
        GlowingObstacle,
        Lava,
        Wall,
        BackWall
    }

    private ColorController colorController;
    private void Awake()
    {
        colorController = FindObjectOfType<ColorController>();
        colorController.OnPaletteChange += ColorController_OnPaletteChange;
    }

    private void ColorController_OnPaletteChange(object sender, ColorController.OnPaletteChangeEventArgs e)
    {
        Material material = GetComponent<MeshRenderer>().material;
        switch (type)
        {
            case ColorizedElementType.BallTrace:
                SetMaterialColor(material, e.palette.ballTraceColor);
                break;
            case ColorizedElementType.GlowingObstacle:
                SetMaterialColor(material, e.palette.glowingObstaclesColor);
                break;
            case ColorizedElementType.Lava:
                SetMaterialColor(material, e.palette.lavaColor);
                break;
            case ColorizedElementType.Wall:
                SetMaterialColor(material, e.palette.wallsColor);
                break;
            case ColorizedElementType.BackWall:
                SetMaterialColor(material, e.palette.backWallColor);
                break;
            default:
                SetMaterialColor(material, e.palette.wallsColor);
                break;
        }
    }

    private void SetMaterialColor(Material material, Color color)
    {
        material.color = color;
        material.SetColor("_EmissionColor", color);
    }

    private void OnDestroy()
    {
        colorController.OnPaletteChange -= ColorController_OnPaletteChange;
    }
}
