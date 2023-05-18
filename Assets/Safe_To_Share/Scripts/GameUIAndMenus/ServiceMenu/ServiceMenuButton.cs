using System;
using Character.PlayerStuff;
using Character.Service;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Safe_To_Share.Scripts.GameUIAndMenus.ServiceMenu
{
    public class ServiceMenuButton : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI title, desc, cost;
        [SerializeField] Button btn;

        BaseService myService;
        void OnDisable() => btn.onClick.RemoveListener(BuyService);

        public static event Action<BaseService> PayAndUse;

        public void Setup(BaseService service, Player player)
        {
            myService = service;
            title.text = service.Title;
            desc.text = service.Desc;
            cost.text = $"Pay {service.Cost}g";
            btn.onClick.AddListener(BuyService);
        }

        void BuyService() => PayAndUse?.Invoke(myService);
    }
}