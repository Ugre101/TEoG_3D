using System.Collections.Generic;
using UnityEngine;

namespace Safe_To_Share.Scripts.AfterBattle
{
    public sealed class SexAnimationManager
    {
        readonly HashSet<int> validHashes = new();
        int? lastHash;
        public void TryPlayAnimation(Animator animator, int hash)
        {
            if (animator == null) return;
            if (validHashes.Contains(hash))
            {
                SetAnimatorBool(animator, hash);
                return;
            }

            foreach (var parameter in animator.parameters)
            {
                if (parameter.nameHash != hash) continue; // Check controller have parameter
                validHashes.Add(hash);
                SetAnimatorBool(animator, hash);
                return;
            }

            if (lastHash.HasValue) // No hit
                animator.SetBool(lastHash.Value, false);
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