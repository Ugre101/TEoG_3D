using System.Collections.Generic;
using Character;
using CustomClasses;
using UnityEngine;

namespace Safe_To_Share.Scripts.AfterBattle
{
    public abstract class VoreAction : AfterBattleBaseAction
    {
        [SerializeField] protected List<DropSerializableObject> needVorePerk = new();
        public bool NeedPerk => needVorePerk.Count > 0;
        public override bool CanUse(BaseCharacter giver, BaseCharacter receiver) => true;
        public override SexActData Use(AfterBattleActor caster, AfterBattleActor target) => data;

        public abstract float ExtraCapacityNeeded(BaseCharacter pred, BaseCharacter prey);
    }
}