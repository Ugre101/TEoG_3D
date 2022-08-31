using System;
using Character;
using Items;
using UnityEngine;

namespace Safe_To_Share.Scripts.Character.Items.Item_Effects
{
    [Serializable]
    public class HealingItemEffects : ItemEffect
    {
        [SerializeField] int hpGain, wpGain;

        public override void OnUse(BaseCharacter user, string itemGuid)
        {
            if (hpGain > 0)
                user.Stats.Health.IncreaseCurrentValue(hpGain);
            if (wpGain > 0)
                user.Stats.WillPower.IncreaseCurrentValue(wpGain);
        }
    }
}