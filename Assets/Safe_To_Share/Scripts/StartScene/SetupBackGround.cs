using System;
using System.Collections;
using Character.LevelStuff;
using Character.PlayerStuff;
using Safe_To_Share.Scripts.Static;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Safe_To_Share.Scripts.StartScene
{
    [Serializable]
    public class SetupBackGround
    {
        [SerializeField] AssetReference poorRef, merchantRef, nobleRef;
        [SerializeField] TMP_Dropdown backGround;
        BasicPerk loadedPerk;
        AsyncOperationHandle<BasicPerk> loadOp;
        StartPerks startPerk = StartPerks.Poor;

        public void Setup() => backGround.SetupTmpDropDown(startPerk, ChangedStartPerk);

        void ChangedStartPerk(int arg0)
        {
            startPerk = UgreTools.IntToEnum(arg0, StartPerks.Merchant);
            LoadStartPerk();
        }

        public void LoadStartPerk()
        {
            if (loadOp.IsValid())
                Addressables.Release(loadOp);
            var perkGuid = startPerk switch
            {
                StartPerks.Poor => poorRef,
                StartPerks.Merchant => merchantRef,
                StartPerks.Noble => nobleRef,
                _ => throw new IndexOutOfRangeException("no such perk exists."),
            };
            loadOp = perkGuid.LoadAssetAsync<BasicPerk>();
            loadOp.Completed += LoadedStartPerk;
        }

        void LoadedStartPerk(AsyncOperationHandle<BasicPerk> obj)
        {
            if (obj.Status == AsyncOperationStatus.Succeeded)
                loadedPerk = obj.Result;
        }

        public IEnumerator GainPerk(Player player)
        {
            if (!loadOp.IsDone)
                yield return loadOp;
            player.LevelSystem.OwnedPerks.Add(loadedPerk);
            loadedPerk.PerkGainedEffect(player);
        }

        enum StartPerks
        {
            Poor,
            Merchant,
            Noble,
        }
    }
}