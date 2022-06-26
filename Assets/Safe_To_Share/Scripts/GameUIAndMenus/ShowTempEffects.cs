using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Character;
using Character.Ailments;
using Character.StatsStuff.Mods;
using GameUIAndMenus.EffectUI;
using Items;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceLocations;

namespace GameUIAndMenus
{
    public class ShowTempEffects : GameMenu
    {
        [SerializeField] List<TempEffectIcon> prePooledPrefabs;
        [SerializeField] TempEffectIcon prefab;
        [SerializeField] SleepTempEffectIcon sleepPrefab;
        [SerializeField] TiredEffectIcon tiredEffectIcon;
        [SerializeField] HungryEffectIcon hungryEffectIcon;
        [SerializeField] MiscTempEffectIcon miscPrefab;
        [SerializeField] Transform content;
        [SerializeField] EffectHoverText hoverText;

        Queue<TempEffectIcon> iconsPool;
        Coroutine loadItemEffects;

        float sleepTier;

        TempEffectIcon GetIcon
        {
            get
            {
                if (iconsPool.Count > 0)
                {
                    var gotten = iconsPool.Dequeue();
                    gotten.gameObject.SetActive(true);
                    return gotten;
                }

                var newPrefab = Instantiate(prefab, content);
                prePooledPrefabs.Add(newPrefab);
                return newPrefab;
            }
        }

        void Start()
        {
            sleepPrefab.HoverInfo += hoverText.ShowHoverText;
            tiredEffectIcon.HoverInfo += hoverText.ShowHoverText;

            sleepPrefab.StopHoverInfo += hoverText.StopHoverText;
            tiredEffectIcon.StopHoverInfo += hoverText.StopHoverText;
        }

        void OnEnable()
        {
            FillIconsPool();
            Dictionary<string, List<TempIntMod>> tempItems = new();
            hoverText.gameObject.SetActive(false);
            AddTempModsFrom(tempItems,
                Player.Stats.GetCharStats.Values.SelectMany(charStat => charStat.Mods.TempBaseStatMods));
            HackGetSleepTier();
            AddTempModsFrom(tempItems,
                Player.Body.BodyStats.Values.SelectMany(bodyStat => bodyStat.Mods.TempBaseStatMods));
            AddTempModsFrom(tempItems,
                Player.PregnancySystem.Fertility.Mods.TempBaseStatMods.Where(tempIntMod =>
                    !tempItems.ContainsKey(tempIntMod.From)));
            AddTempModsFrom(tempItems,
                Player.PregnancySystem.Virility.Mods.TempBaseStatMods.Where(tempIntMod =>
                    !tempItems.ContainsKey(tempIntMod.From)));
            AddTempModsFrom(tempItems, Player.SexStats.MaxCasterOrgasms.Mods.TempBaseStatMods);
            AddTempModsFrom(tempItems, Player.SexStats.BaseMaxArousal.Mods.TempBaseStatMods);
            GetTempVoreMods(tempItems);
            ShowItemEffects(tempItems);

            Player.UpdateAilments += ShowAilments;
            ShowAilments();
        }

        void OnDisable()
        {
            Player.UpdateAilments -= ShowAilments;
            if (loadItemEffects != null)
                StopCoroutine(loadItemEffects);
        }

        void OnDestroy()
        {
            sleepPrefab.HoverInfo -= hoverText.ShowHoverText;
            tiredEffectIcon.HoverInfo -= hoverText.ShowHoverText;

            sleepPrefab.StopHoverInfo -= hoverText.StopHoverText;
            tiredEffectIcon.StopHoverInfo -= hoverText.StopHoverText;
        }

#if UNITY_EDITOR
        void OnValidate()
        {
            if (Application.isPlaying)
                return;
            prePooledPrefabs = new List<TempEffectIcon>(GetComponentsInChildren<TempEffectIcon>(true));
        }
#endif

        void FillIconsPool()
        {
            iconsPool = new Queue<TempEffectIcon>(prePooledPrefabs);
            foreach (TempEffectIcon prePooled in prePooledPrefabs)
                prePooled.gameObject.SetActive(false);
        }

        void ShowAilments()
        {
            if (Hungry.Has(Player))
                hungryEffectIcon.gameObject.SetActive(true);
            else if (Starving.Has(Player))
                hungryEffectIcon.gameObject.SetActive(true);
            else
                hungryEffectIcon.gameObject.SetActive(false);

            tiredEffectIcon.Setup(Player);
        }

        void HackGetSleepTier()
        {
            List<TempIntMod> tempIntMods = Player.Stats.Health.Mods.TempBaseStatMods;
            if (tempIntMods.Exists(m => m.From == SleepExtensions.ModFrom))
                sleepTier = tempIntMods.Find(m => m.From == SleepExtensions.ModFrom).ModValue;
        }


        public override bool BlockIfActive() => false;

        void GetTempVoreMods(IDictionary<string, List<TempIntMod>> tempItems)
        {
            AddTempModsFrom(tempItems, Player.Vore.capacityBoost.TempBaseStatMods);
            AddTempModsFrom(tempItems, Player.Vore.digestionStrength.Mods.TempBaseStatMods);
            AddTempModsFrom(tempItems, Player.Vore.orgasmDrain.Mods.TempBaseStatMods);
            AddTempModsFrom(tempItems, Player.Vore.pleasureDigestion.Mods.TempBaseStatMods);
        }

        static void AddTempModsFrom(IDictionary<string, List<TempIntMod>> dict, IEnumerable<TempIntMod> modsList)
        {
            foreach (var mod in modsList)
                NewMethod(dict, mod);
        }

        static void NewMethod(IDictionary<string, List<TempIntMod>> tempItems, TempIntMod tempIntMod)
        {
            if (tempItems.ContainsKey(tempIntMod.From))
                tempItems[tempIntMod.From].Add(tempIntMod);
            else
                tempItems.Add(tempIntMod.From, new List<TempIntMod> { tempIntMod, });
        }

        void ShowItemEffects(Dictionary<string, List<TempIntMod>> dict)
        {
            HandleSleepEffects(dict);
            foreach (var pair in dict)
                loadItemEffects = StartCoroutine(LoadKeysAndThenItems(pair));
        }

        void HandleSleepEffects(IDictionary<string, List<TempIntMod>> dict)
        {
            if (!dict.ContainsKey(SleepExtensions.ModFrom))
            {
                sleepPrefab.gameObject.SetActive(false);
                return;
            }

            sleepPrefab.gameObject.SetActive(true);
            sleepPrefab.Setup(dict[SleepExtensions.ModFrom], sleepTier);
            dict.Remove(SleepExtensions.ModFrom);
        }

        IEnumerator LoadKeysAndThenItems(KeyValuePair<string, List<TempIntMod>> hashSet)
        {
            var keys = Addressables.LoadResourceLocationsAsync(hashSet.Key);
            yield return keys;
            if (keys.Status != AsyncOperationStatus.Succeeded) yield break;
            foreach (IResourceLocation resourceLocation in keys.Result)
                Addressables.LoadAssetAsync<Item>(resourceLocation).Completed +=
                    item => ItemLoaded(item, hashSet.Value);
        }

        void ItemLoaded(AsyncOperationHandle<Item> obj, List<TempIntMod> mod)
        {
            var icon = GetIcon;
            icon.Setup(obj.Result, mod);
            icon.HoverInfo += hoverText.ShowHoverText;
            icon.StopHoverInfo += hoverText.StopHoverText;
        }
    }
}