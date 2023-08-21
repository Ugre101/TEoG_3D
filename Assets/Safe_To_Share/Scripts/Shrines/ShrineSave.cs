using System;
using UnityEngine;

namespace Safe_To_Share.Scripts.Shrines {
    [Serializable]
    public struct ShrineSave {
        [SerializeField] string chimeraShrine;

        public ShrineSave(ShrinePoints chimeraShrine) => this.chimeraShrine = JsonUtility.ToJson(chimeraShrine);

        public string ChimeraShrine => chimeraShrine;
    }
}