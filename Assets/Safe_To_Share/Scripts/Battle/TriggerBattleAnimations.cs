using System.Collections.Generic;
using UnityEngine;

namespace Battle {
    public enum TriggerBattleAnimations {
        None,
        Dead,
        BasicAttack,
        Tease,
    }

    public enum FloatBattleAnimations {
        None, TakeHealthDamage, TakeWillDamage,
    }

    public static class BattleAnimationDict {
        static readonly int BasicAttack = Animator.StringToHash("basicAttack");
        static readonly int HpDmg = Animator.StringToHash("hpDmg");
        static readonly int WpDmg = Animator.StringToHash("wpDmg");
        static readonly int Dead = Animator.StringToHash("dead");
        static readonly int Tease = Animator.StringToHash("tease");

        public static readonly Dictionary<TriggerBattleAnimations, int> TriggerAnimations = new() {
            { TriggerBattleAnimations.BasicAttack, BasicAttack },
            { TriggerBattleAnimations.Dead, Dead },
            { TriggerBattleAnimations.Tease, Tease },
        };

        public static readonly Dictionary<FloatBattleAnimations, int> FloatAnimations = new() {
            { FloatBattleAnimations.TakeHealthDamage, HpDmg },
            { FloatBattleAnimations.TakeWillDamage, WpDmg },
        };
    }
}