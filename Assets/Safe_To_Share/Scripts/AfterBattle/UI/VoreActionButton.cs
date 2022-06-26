using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

namespace Safe_To_Share.Scripts.AfterBattle.UI
{
    public class VoreActionButton : AfterBattleBaseButton
    {
        [SerializeField] TextMeshProUGUI needed;
        public static event Action<AfterBattleBaseAction> PlayerAction;

        public void ShowNeeded(VoreAction voreAction, float extraCapacityNeeded)
        {
            gameObject.SetActive(true);
            Empty = false;
            icon.sprite = voreAction.Icon;
            title.text = voreAction.Title;
            needed.gameObject.SetActive(true);
        }

        public override void Click()
        {
            if (MyAct == null)
                return;
            PlayerAction?.Invoke(MyAct);
        }

        public override void Setup(AfterBattleBaseAction action)
        {
            base.Setup(action);
            needed.gameObject.SetActive(false);
        }
    }
}