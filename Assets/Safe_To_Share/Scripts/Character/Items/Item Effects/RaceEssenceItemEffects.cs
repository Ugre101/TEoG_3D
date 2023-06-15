using System;
using Character;
using Character.Race.Races;
using Items;
using UnityEngine;

namespace Safe_To_Share.Scripts.Character.Items.Item_Effects {
    [Serializable]
    public class RaceEssenceItemEffects : ItemEffect {
        [SerializeField] BasicRace race;
        [SerializeField, Range(1, 250),] int toGain;

        public override void OnUse(BaseCharacter user, string itemGuid) => user.RaceSystem.AddRace(race, toGain);
    }
}