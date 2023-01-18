using System;
using System.Collections;

namespace Safe_To_Share.Scripts.AfterBattle.UI
{
    public class SexActionButton : AfterBattleBaseButton
    {
        public static event Action<AfterBattleBaseAction> PlayerAction;

        public override void Click()
        {
            if (MyAct == null)
                return;
            PlayerAction?.Invoke(MyAct);
        }
    }
}