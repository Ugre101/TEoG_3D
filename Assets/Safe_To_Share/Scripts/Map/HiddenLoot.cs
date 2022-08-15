using System;
using Character.PlayerStuff;
using Character.PlayerStuff.Currency;
using UnityEngine;
using Random = System.Random;

namespace Map
{
    public class HiddenLoot : MonoBehaviour, IInteractable
    {
        const float GoldRngRange = 0.15f;
        static readonly int OpenLid = Animator.StringToHash("openLid");
        static readonly int CloseLid = Animator.StringToHash("closeLid");

        static readonly Random Rng = new();
        [SerializeField, Range(20, 1000),] int gold = 100;
        [SerializeField, Range(0, 100f),] int chanceToShow = 100;
        [SerializeField] GameObject hideAfterLooted;
        [SerializeField] Animator chestController;
        bool looted;

        void Start()
        {
            if (chanceToShow >= Rng.Next(100))
                chestController.SetTrigger(OpenLid);
            else
                gameObject.SetActive(false);
        }

        public string HoverText(Player player) => looted ? "Empty" : "Loot";

        public void DoInteraction(Player player)
        {
            if (looted)
                return;
            PlayerGold.GoldBag.GainGold(GetGoldGain());
            hideAfterLooted.SetActive(false);
            chestController.SetTrigger(CloseLid);
            looted = true;
            UpdateHoverText?.Invoke(this);
        }

        public event Action<IInteractable> UpdateHoverText;
        public event Action RemoveIInteractableHit;

        int GetGoldGain() => Mathf.RoundToInt(gold * (1f + UnityEngine.Random.Range(-GoldRngRange, GoldRngRange)));
    }
}