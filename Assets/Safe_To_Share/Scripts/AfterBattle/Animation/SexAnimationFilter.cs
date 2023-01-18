using System;
using UnityEngine;

namespace Safe_To_Share.Scripts.AfterBattle.Animation
{
    [Serializable]
    public class SexAnimationFilter
    {
        [field: SerializeField] public SexActionAnimation Animation { get; private set; }
        [field: SerializeField] public Requirements Requirement { get; private set; }
        [SerializeField, Range(1f, 10f)] float xTimes;
        public bool MeetsRequirements(AfterBattleActor caster, AfterBattleActor partner)
        {
            return Requirement switch
            {
                Requirements.PlayerTaller => caster.AvatarScaler.Height / partner.AvatarScaler.Height > xTimes,
                Requirements.PlayerShorter => partner.AvatarScaler.Height / caster.AvatarScaler.Height > xTimes,
                _ => false
            };
        } 
        
        [Serializable] public enum Requirements
        {
            PlayerTaller,
            PlayerShorter,
        }
    }
}