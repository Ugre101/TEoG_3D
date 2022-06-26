using System;
using UnityEngine;

namespace QuestStuff
{
    [Serializable]
    public struct QuestReward
    {
        [SerializeField, Range(0, 10000)] int expGain;
        [SerializeField, Range(0, 10000)] int goldGain;

        public int ExpGain => expGain;

        public int GoldGain => goldGain;
    }
}