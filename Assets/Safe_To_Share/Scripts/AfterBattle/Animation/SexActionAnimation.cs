using System;
using UnityEngine;

namespace Safe_To_Share.Scripts.AfterBattle.Animation {
    [Serializable]
    public struct SexActionAnimation {
        [field: SerializeField] public int GiveAnimationHash { get; private set; }
        [field: SerializeField] public SexPositionPosAndRot GivePos { get; private set; }
        [field: SerializeField] public int ReceiveAnimationHash { get; private set; }
        [field: SerializeField] public SexPositionPosAndRot ReceivePos { get; private set; }
        [field: SerializeField] public bool StayGrounded { get; private set; }
        [field: SerializeField] public float Delay { get; private set; }
    }
}