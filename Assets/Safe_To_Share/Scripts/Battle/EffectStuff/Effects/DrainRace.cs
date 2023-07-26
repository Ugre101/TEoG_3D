using System;
using Character;
using UnityEngine;

namespace Safe_To_Share.Scripts.Battle.EffectStuff.Effects {
    [Serializable]
    public class DrainRace : Effect {
        [SerializeField] int amount = 100;

        public override void UseEffect(BaseCharacter user, BaseCharacter target) {
            if (target.RaceSystem.Race == null)
                return;
            user.RaceSystem.DrainRaceEssence(target.RaceSystem, target.RaceSystem.Race, amount);
        }
    }
}