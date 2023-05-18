using System;
using Character.PlayerStuff;
using Character.PlayerStuff.Currency;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Safe_To_Share.Scripts.GameUIAndMenus.DormUI.UI
{
    public class DormUpgradeButton : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI title, desc, cost;
        [SerializeField] Button btn;
        [SerializeField] TextMeshProUGUI btnText;

        DormAndHome.Dorm.Buildings.Building building;
        Player player;
        public static event Action UpdateDormBuildings;

        public void SubSetup(Player buyer, DormAndHome.Dorm.Buildings.Building toUpgrade, string titleText, string descText)
        {
            title.text = titleText;
            desc.text = descText;
            Setup(buyer, toUpgrade);
        }

        public void Setup(Player buyer, DormAndHome.Dorm.Buildings.Building toUpgrade)
        {
            player = buyer;
            building = toUpgrade;
            btn.onClick.RemoveAllListeners();
            if (building.CanUpgrade)
                btn.onClick.AddListener(Upgrade);
            UpdateCost();
        }

        void UpdateCost()
        {
            cost.text = building.CanUpgrade ? $"{building.UpgradeCost}g" : string.Empty;
            if (building.CanUpgrade)
            {
                btnText.text = building.Level <= 0 ? "Build" : "Upgrade";
                cost.color = PlayerGold.GoldBag.CanAfford(building.UpgradeCost)
                    ? new Color(0.93f, 0.82f, 0.13f)
                    : new Color(0.59f, 0f, 0f);
            }
            else
                btnText.text = "Max";
        }

        void Upgrade()
        {
            if (!building.CanUpgrade)
                btn.onClick.RemoveAllListeners();
            else if (PlayerGold.GoldBag.TryToBuy(building.UpgradeCost))
            {
                building.Upgrade();
                UpdateCost();
                UpdateDormBuildings?.Invoke();
            }
        }
    }
}