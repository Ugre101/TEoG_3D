using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CustomClasses;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Character.LevelStuff
{
    [Serializable]
    public class CharacterLevel : LevelBaseSystem
    {
        protected override int PointsGainedPerLevel => 3;
        public HashSet<BasicPerk> OwnedPerks { get; private set; } = new();

        public IEnumerable<BasicPerk> PerksOfType(PerkType type) => OwnedPerks.Where(perk => perk.PerkType == type);
        public SerializableScriptableObjectSaves PerkSave() => new(OwnedPerks);

        public IEnumerator Load(SerializableScriptableObjectSaves toLoad)
        {
            OwnedPerks = new HashSet<BasicPerk>();
            if (toLoad.SavedGuids == null)
                yield break;
            foreach (AsyncOperationHandle<BasicPerk> op in toLoad.SavedGuids.Select(Addressables
                         .LoadAssetAsync<BasicPerk>))
            {
                yield return op;
                if (op.Status == AsyncOperationStatus.Succeeded)
                    OwnedPerks.Add(op.Result);
            }
        }
    }
}