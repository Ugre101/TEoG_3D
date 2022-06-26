using System;
using UnityEngine;

namespace Character.CreateCharacterStuff
{
    [Serializable]
    public struct CanTakeEnemyHome
    {
        [SerializeField] bool canTake;
        [SerializeField] int orgasmsNeeded;

        public bool CanTake => canTake;
        public int OrgasmsNeeded => orgasmsNeeded;
    }
}