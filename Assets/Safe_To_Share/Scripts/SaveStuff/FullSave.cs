using System;
using UnityEngine;

namespace SaveStuff {
    [Serializable]
    public struct FullSave {
        [SerializeField] SaveSummary summary;
        [SerializeField] Save save;

        public FullSave(SaveSummary summary, Save save) {
            this.summary = summary;
            this.save = save;
        }

        public SaveSummary Summary => summary;

        public Save Save => save;
    }
}