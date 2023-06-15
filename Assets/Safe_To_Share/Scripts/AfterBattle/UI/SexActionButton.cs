using System;

namespace Safe_To_Share.Scripts.AfterBattle.UI {
    public sealed class SexActionButton : AfterBattleBaseButton {
        public static event Action<AfterBattleBaseAction> PlayerAction;

        public override void Click() {
            if (HasAct is false)
                return;
            PlayerAction?.Invoke(MyAct);
        }
    }
}