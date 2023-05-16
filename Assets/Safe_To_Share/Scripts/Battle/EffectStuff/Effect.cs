using System;
using System.Collections.Generic;
using System.Linq;
using Battle.SkillsAndSpells;
using Character;
using Character.StatsStuff.Mods;
using Safe_to_Share.Scripts.CustomClasses;
using UnityEngine;

namespace Safe_To_Share.Scripts.Battle.EffectStuff
{
    [Serializable]
    public abstract class Effect
    {
        [SerializeField] bool active;
        [SerializeField] int value;
        [SerializeField] RngValue rngValue;
        [SerializeField] List<AffectedByStat> affectedByStats = new();
        [SerializeField] protected ModType modType = ModType.Flat;

        public bool Active => active;


        public virtual void UseEffect(BaseCharacter user) => UseEffect(user, user);

        public abstract void UseEffect(BaseCharacter user, BaseCharacter target);

        public float FinalValue(BaseCharacter user, float baseValue) =>
            modType == ModType.Flat ? Value(user) : Percent(user, baseValue);

        public int FinalIntValue(BaseCharacter user, int baseValue) =>
            modType == ModType.Flat ? IntValue(user) : IntPercent(user, baseValue);


        protected float Value(BaseCharacter user) => value * StatModValue(user) * rngValue.GetRandomValue;

        protected int IntValue(BaseCharacter user) => Mathf.RoundToInt(Value(user));

        protected float Percent(BaseCharacter user, float baseValue) => baseValue * (Value(user) / 100f);
        protected int IntPercent(BaseCharacter user, int baseValue) => Mathf.RoundToInt(Percent(user, baseValue));

        protected float StatModValue(BaseCharacter user) => 1f + (affectedByStats.Count == 0
            ? 0 : affectedByStats.Sum(v => (float)user.Stats.GetCharStats[v.StatType].Value / v.DivValue));
    }
}