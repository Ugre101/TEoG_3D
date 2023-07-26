using System;
using Character;
using Character.Race.Races;
using UnityEngine;

namespace Safe_To_Share.Scripts.Battle.EffectStuff.Effects {
    [Serializable]
    public class TransformToRace : Effect {
        [SerializeField] BasicRace race;
        [SerializeField] int amount = 100;

        public override void UseEffect(BaseCharacter user, BaseCharacter target) {
            target.RaceSystem.AddRace(race, amount);
        }
    }
}