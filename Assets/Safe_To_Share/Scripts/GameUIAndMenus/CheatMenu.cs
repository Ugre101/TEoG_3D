using Character.PlayerStuff;
using Character.PlayerStuff.Currency;
using CustomClasses;
using UnityEngine;
using UnityEngine.UI;

namespace Safe_To_Share.Scripts.GameUIAndMenus
{
    public class CheatMenu : MonoBehaviour,ICancelMeBeforeOpenPauseMenu
    {
        [SerializeField] Button expBtn, goldBtn, mascBtn, femiBtn;
        Player player;
        void Start()
        {
            expBtn.onClick.AddListener(ExpCheat);
            goldBtn.onClick.AddListener(GoldCheat);
            mascBtn.onClick.AddListener(MascCheat);
            femiBtn.onClick.AddListener(FemiCheat);
        }

        public void Enter(Player parPlayer)
        {
            gameObject.SetActive(true);
            this.player = parPlayer;
        }

        void FemiCheat() => player.Essence.Femininity.Amount += 200;

        void MascCheat() => player.Essence.Masculinity.Amount += 200;

        void GoldCheat() => PlayerGold.GoldBag.GainGold(200);

        void ExpCheat() => player.LevelSystem.GainExp(200);

        public bool BlockIfActive()
        {
            if (gameObject.activeInHierarchy)
            {
                gameObject.SetActive(false);
                return true;
            }

            return false;
        }
    }
}