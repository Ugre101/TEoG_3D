using System;
using System.Collections.Generic;

namespace Safe_To_Share.Scripts.Static
{
    public static class GameUIManager
    {
        public static event Action<bool> HideGameUI;
        public static void TriggerHideGameUI(bool hide) => HideGameUI?.Invoke(hide);

        public static readonly List<IBlockGameUI> BlockList = new();

        
    }
}