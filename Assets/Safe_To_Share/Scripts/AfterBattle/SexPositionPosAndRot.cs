using System;
using AvatarStuff;
using UnityEngine;

namespace Safe_To_Share.Scripts.AfterBattle
{
    [Serializable]
    public struct SexPositionPosAndRot
    {
        [field: SerializeField] public  AvatarKeyAreas.Area KeyArea { get; private set; }
        [field: SerializeField] public ActorDirection TowardsPartner { get; private set; }
      
    }
}