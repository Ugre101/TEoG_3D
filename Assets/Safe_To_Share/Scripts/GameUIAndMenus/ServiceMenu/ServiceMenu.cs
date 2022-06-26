using System.Collections.Generic;
using Character.PlayerStuff.Currency;
using Character.Service;
using Currency;
using Currency.UI;
using TMPro;
using UnityEngine;

namespace GameUIAndMenus.ServiceMenu
{
    public class ServiceMenu : GameMenu
    {
        [SerializeField] TextMeshProUGUI title;
        [SerializeField] ServiceMenuButton prefab;
        [SerializeField] Transform content;
        [SerializeField] HaveGold haveGold;

        void OnDisable()
        {
            PlayerGold.GoldBag.GoldAmountChanged -= haveGold.GoldChanged;
            ServiceMenuButton.PayAndUse -= BuyThisService;
        }


        public void Setup(string titleText, List<BaseService> services)
        {
            title.text = titleText;
            ShowServices(services);
            haveGold.GoldChanged(PlayerGold.GoldBag.Gold);
            PlayerGold.GoldBag.GoldAmountChanged += haveGold.GoldChanged;
            ServiceMenuButton.PayAndUse += BuyThisService;
        }

        void ShowServices(List<BaseService> services)
        {
            content.KillChildren();
            foreach (BaseService service in services) 
                Instantiate(prefab, content).Setup(service, Player);
        }

        void BuyThisService(BaseService obj)
        {
            if (PlayerGold.GoldBag.TryToBuy(obj.Cost))
                obj.OnUse(Player);
        }
    }
}