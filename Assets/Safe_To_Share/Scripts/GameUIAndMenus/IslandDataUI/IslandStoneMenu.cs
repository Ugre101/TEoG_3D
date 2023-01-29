using Character.PlayerStuff;
using CustomClasses;
using Items;
using Safe_To_Share.Scripts.Static;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Safe_To_Share.Scripts.GameUIAndMenus.IslandDataUI
{
    public class IslandStoneMenu : MonoBehaviour, ICancelMeBeforeOpenPauseMenu, IBlockGameUI
    {
        [SerializeField] IslandStoneOption[] options;
        [SerializeField] AssetReference emptyCraftingCrystal;
        [SerializeField] AmountOfItem amountOfStones;
#if UNITY_EDITOR
        void OnValidate()
        {
            if (Application.isPlaying) return;
            options = GetComponentsInChildren<IslandStoneOption>();
        }
#endif
        void OnDisable()
        {
            GameUIManager.BlockList.Remove(this);
            emptyCraftingCrystal.ReleaseAsset();
        }

        public bool BlockIfActive()
        {
            if (gameObject.activeSelf)
            {
                gameObject.SetActive(false);
                return true;
            }

            return false;
        }

        public async void Open(Player player)
        {
            GameUIManager.BlockList.Add(this);
            var op = emptyCraftingCrystal.LoadAssetAsync<Item>();
            await op.Task;
            gameObject.SetActive(true);
            foreach (var stoneOption in options)
                stoneOption.Setup(player, op.Result.Guid);
            amountOfStones.Setup(player, op.Result);
        }
    }
}