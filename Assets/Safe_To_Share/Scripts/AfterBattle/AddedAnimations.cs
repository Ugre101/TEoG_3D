using System.Collections.Generic;
using UnityEngine;

namespace Safe_To_Share.Scripts.AfterBattle
{
    public static class AddedAnimations
    {
        public enum SexAnimations
        {
            None,
            Missionary,
        }

        static Dictionary<SexAnimations, int> hashedAnimations;

        public static Dictionary<SexAnimations, int> GetAnimationHash =>
            hashedAnimations ??= new Dictionary<SexAnimations, int>
            {
                { SexAnimations.Missionary, Animator.StringToHash("Missionary") },
            };
    }
}