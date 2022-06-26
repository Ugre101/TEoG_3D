using System;
using Character.PlayerStuff;
using Character.Service;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GameUIAndMenus.ServiceMenu
{
    public class ServiceMenuButton : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI title, desc, cost;
        [SerializeField] Button btn;

        public static event Action<BaseService> PayAndUse;

        BaseService myService;
        public void Setup(BaseService service,Player player)
        {
            myService = service;
            title.text = service.Title;
            desc.text = service.Desc;
            cost.text = $"Pay {service.Cost}g";
            btn.onClick.AddListener(BuyService);
        }
        void OnDisable() => btn.onClick.RemoveListener(BuyService);
        void BuyService() => PayAndUse?.Invoke(myService);
    }
}