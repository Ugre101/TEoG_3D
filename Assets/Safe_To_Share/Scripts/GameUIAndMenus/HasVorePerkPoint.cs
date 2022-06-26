using System.Collections;
using System.Collections.Generic;
using GameUIAndMenus;
using SaveStuff;
using UnityEngine;
using UnityEngine.UI;

public class HasVorePerkPoint : GameMenu
{
    
    [SerializeField] Color no = Color.white, yes = Color.magenta;
    [SerializeField] Image image;

    public override bool BlockIfActive() => false;

    void OnEnable()
    {
        Refresh();
        Player.Vore.Level.PerkPointsChanged += CheckGained;
        LoadManager.LoadedSave += Refresh;
    }


    void OnDisable()
    {
        Player.Vore.Level.PerkPointsChanged -= CheckGained;
        LoadManager.LoadedSave -= Refresh;
    }

    void CheckGained(int obj) => image.color = obj > 0 ? yes : no;

    void Refresh() => image.color = Player.Vore.Level.Points > 0 ? yes : no;
}
