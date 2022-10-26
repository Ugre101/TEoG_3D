using System;
using UnityEngine;

namespace Safe_To_Share.Scripts.AfterBattle.Vore
{
    [Serializable]
    public struct SexActionAnimation
    {
        [field: SerializeField] public string GiveAnimationName { get; private set; }
        [field: SerializeField] public Vector3 GiveOffset { get; private set; }
        [field: SerializeField] public string ReceiveAnimationName { get; private set; }
        [field: SerializeField] public Vector3 ReceiveOffset { get; private set; }
    }
}