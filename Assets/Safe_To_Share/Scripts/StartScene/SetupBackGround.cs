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
        [SerializeField] private AssetReference poorRef, merchantRef, nobleRef;
        [SerializeField] private TMP_Dropdown backGround;
        private StartPerks startPerk = StartPerks.Poor;
        private BasicPerk loadedPerk;
        private AsyncOperationHandle<BasicPerk> loadOp;

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

        private void LoadedStartPerk(AsyncOperationHandle<BasicPerk> obj)
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

        private enum StartPerks
        {
            Poor,
            Merchant,
            Noble,
        }
    }
}