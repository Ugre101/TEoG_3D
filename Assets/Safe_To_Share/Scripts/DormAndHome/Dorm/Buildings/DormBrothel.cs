using System;
using System.Collections.Generic;
using Character.EssenceStuff;
using Character.PlayerStuff.Currency;
using Currency;
using UnityEngine;
using Random = System.Random;

namespace DormAndHome.Dorm.Buildings
{
    [Serializable]
    public class DormBrothel : Building
    {
        [Serializable]
        public enum BrothelSettings
        {
            Closed,
            ServiceAll, // More gold and essence
            ServiceMasculine, // Less gold, more masc and no femi
            ServiceFeminine, // Less gold, more femi and no masc
        }

        public const string WorkTitle = "Whore";
        [SerializeField] BrothelSettings setting = BrothelSettings.ServiceAll;

        Random rng = new();

        protected override int[] UpgradeCosts => new[] { 250, 500, 1000, };

        public BrothelSettings Setting
        {
            get => setting;
            set => setting = value;
        }

        public override void TickBuildingEffect(List<DormMate> dormMates)
        {
            if (Level <= 0)
                return;
            foreach (DormMate dormMate in dormMates)
                TickBrothel(dormMate);
        }

        void TickBrothel(DormMate dormMate)
        {
            switch (Setting)
            {
                case BrothelSettings.Closed:
                    break;
                case BrothelSettings.ServiceAll:
                    dormMate.GainFemi(rng.Next(4 * Level));
                    dormMate.GainMasc(rng.Next(4 * Level));
                    PlayerGold.GoldBag.GainGold(rng.Next(15 * Level));
                    break;
                case BrothelSettings.ServiceMasculine:
                    dormMate.GainMasc(rng.Next(7 * Level));
                    PlayerGold.GoldBag.GainGold(rng.Next(13 * Level));
                    break;
                case BrothelSettings.ServiceFeminine:
                    dormMate.GainFemi(rng.Next(7 * Level));
                    PlayerGold.GoldBag.GainGold(rng.Next(13 * Level));
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}