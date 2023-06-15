using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Character.LevelStuff;
using CustomClasses;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Character.VoreStuff {
    [Serializable]
    public class VoreLevel : LevelBaseSystem {
        protected override int PointsGainedPerLevel => 1;
        public HashSet<VorePerk> OwnedPerks { get; private set; } = new();

        public IEnumerable<VorePerk> PerksOfType(PerkType type) => OwnedPerks.Where(perk => perk.PerkType == type);
        public SerializableScriptableObjectSaves Save() => new(OwnedPerks);

        public IEnumerator Load(SerializableScriptableObjectSaves toLoad) {
            OwnedPerks = new HashSet<VorePerk>();
            if (toLoad.SavedGuids == null)
                yield break;
            foreach (var op in toLoad.SavedGuids.Select(
                         Addressables.LoadAssetAsync<VorePerk>)) {
                yield return op;
                if (op.Status == AsyncOperationStatus.Succeeded)
                    OwnedPerks.Add(op.Result);
            }
        }

        public void LoadMyPerkAssets() {
            foreach (var perk in OwnedPerks) Addressables.LoadAssetAsync<VorePerk>(perk.Guid);
        }
    }
}