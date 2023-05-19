using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Character;
using Character.Ailments;
using Character.StatsStuff.Mods;
using Items;
using Safe_To_Share.Scripts.GameUIAndMenus.EffectUI;
using SaveStuff;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Pool;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Safe_To_Share.Scripts.GameUIAndMenus
{
    public sealed class ShowTempEffects : GameMenu
    {
        [SerializeField] TempEffectIcon prefab;
        [SerializeField] SleepTempEffectIcon sleepPrefab;
        [SerializeField] TiredEffectIcon tiredEffectIcon;
        [SerializeField] HungryEffectIcon hungryEffectIcon;
        [SerializeField] MiscTempEffectIcon miscPrefab;
        [SerializeField] Transform content;
        [SerializeField] EffectHoverText hoverText;

        ObjectPool<TempEffectIcon> iconsPool;

        Coroutine loadItemEffects;

        float sleepTier;

        ObjectPool<TempEffectIcon> IconsPool => iconsPool ??= new ObjectPool<TempEffectIcon>(CreateFunc, ActionOnGet, ActionOnRelease, ActionOnDestroy);


        void Start()
        {
            sleepPrefab.HoverInfo += hoverText.ShowHoverText;
            tiredEffectIcon.HoverInfo += hoverText.ShowHoverText;

            sleepPrefab.StopHoverInfo += hoverText.StopHoverText;
            tiredEffectIcon.StopHoverInfo += hoverText.StopHoverText;
        }

        void OnEnable()
        {
            hoverText.gameObject.SetActive(false);
            ShowTempModsEffects();
            ShowAilments();
            holder.RePlaced += ReBind;
            Player.UpdateAilments += ShowAilments;
            LoadManager.LoadedSave += Loaded;
        }

        void ReBind()
        {
            // TODO
        }


        void OnDisable()
        {
            Player.UpdateAilments -= ShowAilments;
            holder.RePlaced -= ReBind;
            LoadManager.LoadedSave -= Loaded;
            if (loadItemEffects != null)
                StopCoroutine(loadItemEffects);
        }

        void OnDestroy()
        {
            IconsPool.Dispose();
            sleepPrefab.HoverInfo -= hoverText.ShowHoverText;
            tiredEffectIcon.HoverInfo -= hoverText.ShowHoverText;

            sleepPrefab.StopHoverInfo -= hoverText.StopHoverText;
            tiredEffectIcon.StopHoverInfo -= hoverText.StopHoverText;
        }

        static void ActionOnDestroy(TempEffectIcon obj) => Destroy(obj.gameObject);


        static void ActionOnRelease(TempEffectIcon obj)
        {
            obj.Clear();
            obj.gameObject.SetActive(false);
        }

        static void ActionOnGet(TempEffectIcon obj) => obj.gameObject.SetActive(true);

        TempEffectIcon CreateFunc()
        {
            var obj = Instantiate(prefab, content);
            obj.pool = IconsPool;
            return obj;
        }
        void Loaded()
        {
            if (loadItemEffects != null)
                StopCoroutine(loadItemEffects);
            foreach (Transform icon in content.transform)
            {
                icon.gameObject.SetActive(false);
            }
            ShowTempModsEffects();
            ShowAilments();
        }

        void ShowTempModsEffects()
        {
            Dictionary<string, List<TempIntMod>> tempItems = new();
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
            loadItemEffects = StartCoroutine(ShowItemEffects(tempItems));
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
            var tempIntMods = Player.Stats.Health.Mods.TempBaseStatMods;
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

        IEnumerator ShowItemEffects(Dictionary<string, List<TempIntMod>> dict)
        {
            HandleSleepEffects(dict);
            foreach (var pair in dict)
                yield return LoadKeysAndThenItems(pair);
            loadItemEffects = null;
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
            var keys = Addressables.LoadResourceLocationsAsync(hashSet.Key, typeof(Item));
            yield return keys;
            if (keys.Status != AsyncOperationStatus.Succeeded) yield break;
            if (keys.Result.Count <= 0) yield break;
            var item = Addressables.LoadAssetAsync<Item>(keys.Result[0]);
            yield return item;
            if (item.Status != AsyncOperationStatus.Succeeded)
                yield break;
            var icon = IconsPool.Get();
            icon.Setup(item.Result, hashSet.Value);
            icon.HoverInfo += hoverText.ShowHoverText;
            icon.StopHoverInfo += hoverText.StopHoverText;
        }
    }
}