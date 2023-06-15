using System;
using UnityEngine;

namespace Character.EnemyStuff {
    [Serializable]
    public struct EnemySave {
        [field: SerializeField] public CharacterSave CharacterSave { get; private set; }
        [field: SerializeField] public Vector3 Position { get; private set; }
    }
}