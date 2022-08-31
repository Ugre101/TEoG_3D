using Character;
using Character.Race;
using Character.StatsStuff.Mods;
using Safe_to_Share.Scripts.CustomClasses;
using UnityEngine;

namespace Items
{
    [CreateAssetMenu(fileName = "Item", menuName = "Items/NewItem")]
    public class Item : SObjSavableTitleDescIcon
    {
        // [SerializeField] EffectsTree effectsTree = new EffectsTree();
        [SerializeField] Vector2 size = new(1, 1);
        [SerializeField] bool unlimitedUse;
        [SerializeField] int value;
        [SerializeField] bool canSell = true;
        [SerializeField] ItemEffectsTree effectsTree = new();
        [SerializeField, TextArea,] string tempEffectDesc;
        [SerializeField] bool updateInventoryAfterUse;
        [Header("Requirements")]
        [SerializeField] RaceReq raceReq;
        // public IEnumerable<Effect> Effects => effectsTree.ActiveEffects;

        public Vector2 Size => size;

        public int Value => value;

        public bool CanSell => canSell;

        public string TempEffectDesc => tempEffectDesc;

        public bool UnlimitedUse => unlimitedUse;

        public bool UpdateInventoryAfterUse => updateInventoryAfterUse;

        public virtual bool CanUse(BaseCharacter user) => raceReq.IsRace(user.RaceSystem);

        public virtual void Use(BaseCharacter user)
        {
            if (!CanUse(user))
                return;
            foreach (ItemEffect activeEffect in effectsTree.ActiveEffects)
                activeEffect.OnUse(user, Guid);
            //  foreach (Effect effect in Effects) effect.UseEffect(user);
        }

        protected TempIntMod TempModFromItem(TempIntMod tempIntMod) => new(tempIntMod.HoursLeft,
            tempIntMod.ModValue, Title, tempIntMod.ModType);
    }
}