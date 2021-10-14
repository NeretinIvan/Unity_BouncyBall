using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorController : MonoBehaviour
{
    public event System.EventHandler<OnPaletteChangeEventArgs> OnPaletteChange;

    public class OnPaletteChangeEventArgs
    {
        public ColorPalette palette;
    }

    [SerializeField] private List<ColorPalette> colorPaletteStorage;
    private ColorPalette lastPalette;

    private void Awake()
    {
        FindObjectOfType<GameLogicController>().OnDifficultyUpdate += ColorController_OnDifficultyUpdate;
        OnPaletteChange += ColorController_OnPaletteChange;
    }

    private void ColorController_OnPaletteChange(object sender, OnPaletteChangeEventArgs e)
    {
        lastPalette = e.palette;
    }

    public void ChangePalette(ColorPalette palette)
    {
        OnPaletteChange.Invoke(this, new OnPaletteChangeEventArgs { palette = palette });
    }

    private void ColorController_OnDifficultyUpdate(object sender, GameLogicController.DifficultyUpdateEventArgs e)
    {
        ChangePalette(PickPalette(e.Difficulty, false));
    }

    private ColorPalette PickPalette(int difficultyLevel, bool includeLastPalette)
    {
        List<ColorPalette> appearingPalettes = new List<ColorPalette>();
        foreach(ColorPalette palette in colorPaletteStorage)
        {
            if ((palette.appearsInDifficulty.from <= difficultyLevel) && (palette.appearsInDifficulty.to >= difficultyLevel))
            {
                if (!includeLastPalette && (palette == lastPalette)) continue;
                appearingPalettes.Add(palette);
            }
        }

        int indexPicked = Random.Range(0, appearingPalettes.Count);
        return appearingPalettes[indexPicked];
    }
}
