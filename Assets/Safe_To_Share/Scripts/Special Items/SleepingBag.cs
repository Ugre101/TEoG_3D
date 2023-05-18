using Character;
using Items;
using Safe_To_Share.Scripts.Static;
using UnityEngine;

namespace Safe_To_Share.Scripts.Special_Items
{
    [CreateAssetMenu(menuName = "Items/Special Items/Create SleepingBag", fileName = "SleepingBag", order = 0)]
    public sealed class SleepingBag : Item
    {
        [SerializeField] int sleepQuality = 50;
        public override void Use(BaseCharacter user)
        {
            base.Use(user);
            user.BaseSleep(sleepQuality);
            GameUIManager.TriggerSleepEffect();
        }
    }
}