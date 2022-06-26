using Character.PlayerStuff.Currency;
using Currency;
using GameUIAndMenus;
using UnityEngine;
using UnityEngine.UI;

public class CheatMenu : GameMenu
{
    [SerializeField] Button expBtn, goldBtn, mascBtn, femiBtn;

    void Start()
    {
        expBtn.onClick.AddListener(ExpCheat);
        goldBtn.onClick.AddListener(GoldCheat);
        mascBtn.onClick.AddListener(MascCheat);
        femiBtn.onClick.AddListener(FemiCheat);
    }

    void FemiCheat() => Player.Essence.Femininity.Amount += 200;

    void MascCheat() => Player.Essence.Masculinity.Amount += 200;

    void GoldCheat() => PlayerGold.GoldBag.GainGold(200);

    void ExpCheat() => Player.LevelSystem.GainExp(200);

    public override bool BlockIfActive()
    {
        if (gameObject.activeInHierarchy)
        {
            gameObject.SetActive(false);
            return true;
        }

        return false;
    }
}