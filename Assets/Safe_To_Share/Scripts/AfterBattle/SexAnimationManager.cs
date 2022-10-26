using System.Collections.Generic;
using UnityEngine;

namespace Safe_To_Share.Scripts.AfterBattle
{
    public class SexAnimationManager 
    {
        readonly Dictionary<string, int> dict = new();
        int? lastHash;
        public void TryPlayAnimation(Animator animator, string ani)
        {
            if (animator == null) return;
            if (dict.TryGetValue(ani, out int hash))
                SetAnimatorBool(animator, hash);
            else
            {
                foreach (var parameter in animator.parameters)
                {
                    if (parameter.name != ani) continue;
                    dict.TryAdd(ani, parameter.nameHash);
                    SetAnimatorBool(animator, parameter.nameHash);
                    break;
                }
            }
        }

        public void Clear() => dict.Clear();

        void SetAnimatorBool(Animator animator, int hash)
        {
            if (lastHash.HasValue)
            {
                if (lastHash.Value == hash)
                    return;
                animator.SetBool(lastHash.Value, false);
            }

            animator.SetBool(hash, true);
            lastHash = hash;
        }
    }
}