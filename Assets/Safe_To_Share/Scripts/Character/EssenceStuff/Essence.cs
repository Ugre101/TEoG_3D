using System;
using UnityEngine;

namespace Character.EssenceStuff
{
    [Serializable]
    public class Essence
    {
        [SerializeField] int amount;

        public int Amount
        {
            get => amount;
            set
            {
                amount = Mathf.Max(0, value);
                EssenceChange?.Invoke(amount);
            }
        }

        public event Action<int> EssenceChange;

        public int LoseEssence(int toLose)
        {
            int have = Mathf.Min(toLose, Amount);
            Amount -= toLose;
            return have;
        }
    }
}