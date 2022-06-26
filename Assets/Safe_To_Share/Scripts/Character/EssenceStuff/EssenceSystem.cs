using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CustomClasses;
using Safe_to_Share.Scripts.CustomClasses;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Character.EssenceStuff
{
    [Serializable]
    public class EssenceSystem : ITickHour
    {
        [SerializeField] Essence femi = new(), masculinity = new();
        [SerializeField] StableEssence stableEssence = new(0);
        [SerializeField] EssenceOptions essenceOptions = new();
        Dictionary<EssenceType, Essence> allEssence;
        public List<EssencePerk> EssencePerks { get; private set; } = new();
        public Essence Femininity => femi;
        public Essence Masculinity => masculinity;

        public Dictionary<EssenceType, Essence> GetEssence => allEssence ??= new Dictionary<EssenceType, Essence>
        {
            { EssenceType.Femi, femi },
            { EssenceType.Masc, masculinity },
        };

        public StableEssence StableEssence => stableEssence;

        public BaseConstIntStat DrainAmount { get; private set; } = new(20);
        public BaseConstIntStat GiveAmount { get; private set; } = new(0);

        public EssenceOptions EssenceOptions => essenceOptions;

        public bool TickHour(int ticks = 1)
        {
            bool change = StableEssence.Mods.TickHour(ticks);
            if (DrainAmount.Mods.TickHour(ticks))
                change = true;
            if (GiveAmount.Mods.TickHour(ticks))
                change = true;
            return change;
        }

        public IEnumerator Load(SerializableScriptableObjectSaves toLoad)
        {
            EssencePerks = new List<EssencePerk>();
            DrainAmount = new BaseConstIntStat(20);
            GiveAmount = new BaseConstIntStat(0);
            if (toLoad.SavedGuids == null)
                yield break;
            foreach (AsyncOperationHandle<EssencePerk> op in toLoad.SavedGuids.Select(Addressables
                         .LoadAssetAsync<EssencePerk>))
            {
                yield return op;
                if (op.Status == AsyncOperationStatus.Succeeded)
                    EssencePerks.Add(op.Result);
            }
        }

        public SerializableScriptableObjectSaves Save() => new(EssencePerks);

        public void LoadMyPerkAssets()
        {
            foreach (var perk in EssencePerks)
                Addressables.LoadAssetAsync<EssencePerk>(perk.Guid);
        }
    }
}