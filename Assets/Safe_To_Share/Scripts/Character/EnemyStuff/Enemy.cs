using System;
using System.Collections.Generic;
using Character.CreateCharacterStuff;
using Character.DefeatScenarios.Custom;
using Character.IslandData;
using UnityEngine;

namespace Character.EnemyStuff
{
    [Serializable]
    public class Enemy : BaseCharacter
    {
        [SerializeField] BattleReward reward;
        [SerializeField] CanTakeEnemyHome canTake;

        public Enemy(CreateEnemy character) : base(character.Character)
        {
            reward = character.BattleReward;
            canTake = character.CanTake;
            LoseScenarios = character.LoseScenarios;
            CustomLoseScenarios = character.CustomLoseScenarios;
            WantBodyMorph = character.GiveRandomBodyMorphs;
        }

        public Enemy(CreateEnemy character, Islands islands) : base(character.Character, islands)
        {
            reward = character.BattleReward;
            canTake = character.CanTake;
            LoseScenarios = character.LoseScenarios;
            CustomLoseScenarios = character.CustomLoseScenarios;
            WantBodyMorph = character.GiveRandomBodyMorphs;
        }

        public BattleReward Reward => reward;

        public CanTakeEnemyHome CanTake => canTake;
        public string LoseScenarios { get; }
        public List<CustomLoseScenario> CustomLoseScenarios { get; }
        public bool WantBodyMorph { get; set; }
        public event Action Removed;
        public void GotRemoved() => Removed?.Invoke();
        public void NullifyRemoved() => Removed = null;
        public bool CanTakeHome() => CanTake.CanTake && CanTake.OrgasmsNeeded <= SexStats.TotalOrgasms;
    }
}