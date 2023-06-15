using System;
using UnityEngine;

namespace Safe_To_Share.Scripts.CustomClasses {
    [Serializable]
    public class AddAmountOf<T> {
        [field: SerializeField] public T Value { get; private set; }
        [field: SerializeField] public int Amount { get; private set; } = 1;
    }
}