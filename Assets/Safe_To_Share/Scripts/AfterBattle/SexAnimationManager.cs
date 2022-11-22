using System.Collections.Generic;
using UnityEngine;

namespace Safe_To_Share.Scripts.AfterBattle
{
    public class SexAnimationManager
    {
        readonly HashSet<int> validHashes = new();
        int? lastHash;
        public void TryPlayAnimation(Animator animator, int hash)
        {
            if (animator == null) return;
            if (validHashes.Contains(hash))
                SetAnimatorBool(animator, hash);
            else
            {
                foreach (var parameter in animator.parameters)
                {
                    if (parameter.nameHash == hash) // Check controller have parameter
                    {
                        validHashes.Add(hash);
                        SetAnimatorBool(animator, hash);
                        break;
                    }
                }
            }
        }

        public void Clear() => validHashes.Clear();

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