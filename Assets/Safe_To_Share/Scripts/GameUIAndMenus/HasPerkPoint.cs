using GameUIAndMenus;
using SaveStuff;
using UnityEngine;
using UnityEngine.UI;

public class HasPerkPoint : GameMenu
{
    [SerializeField] Color no, yes;
    [SerializeField] Image image;

    void OnEnable()
    {
        Refresh();
        LoadManager.LoadedSave += Refresh;
        Player.LevelSystem.PerkPointsChanged += CheckGained;
    }


    void OnDisable()
    {
        LoadManager.LoadedSave -= Refresh;
        Player.LevelSystem.PerkPointsChanged -= CheckGained;
    }

    public override bool BlockIfActive() => false;
    void CheckGained(int obj) => image.color = obj > 0 ? yes : no;

    void Refresh() => image.color = Player.LevelSystem.Points > 0 ? yes : no;
}