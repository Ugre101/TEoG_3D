using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Character.Race {
    [Serializable]
    public class RaceReq {
        [SerializeField] bool hasRaceReq;
        [SerializeField] List<AssetReference> races;

        public bool IsRace(RaceSystem raceSystem) {
            if (hasRaceReq is false || races == null || races.Count == 0) return true;
            return races.Exists(r => r.AssetGUID == raceSystem.Race.Guid);
        }
    }
}