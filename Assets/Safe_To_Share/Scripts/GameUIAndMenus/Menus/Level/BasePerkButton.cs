using Character.LevelStuff;
using Safe_To_Share.Scripts.Holders;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Safe_To_Share.Scripts.GameUIAndMenus.Menus.Level {
    public abstract class BasePerkButton : BaseLevelButton {
        [SerializeField, HideInInspector,] protected string guid;
        protected bool Have;
        protected BasicPerk loaded;

        void LoadedPerk(AsyncOperationHandle<BasicPerk> obj) {
            loaded = obj.Result;
            CanAfford(PlayerHolder.Player.LevelSystem.Points);
            HasPerk(obj.Result);
        }

        public override void Setup(PlayerHolder player) {
            PlayerHolder = player;
            Addressables.LoadAssetAsync<BasicPerk>(guid).Completed += LoadedPerk;
        }


        protected override void CanAfford(int obj) => Afford = loaded != null && loaded.Cost <= obj;

        protected void HasPerk(BasicPerk result) {
            var hasPerk = PlayerHolder.Player.LevelSystem.OwnedPerks.Contains(result);
            HaveFade(hasPerk);
        }

        protected void HaveFade(bool value) {
            Have = value;
            var color = rune.color;
            color.a = GetAplha(value);
            rune.color = color;
        }

        float GetAplha(bool havePerk) => havePerk ? 1f : loaded.MeetsRequirements(PlayerHolder.Player) ? 0.4f : 0.15f;
#if UNITY_EDITOR
        [SerializeField] BasicPerk basicPerk;

        void OnValidate() {
            guid = basicPerk != null ? basicPerk.Guid : string.Empty;
            if (rune == null || basicPerk == null)
                return;
            rune.sprite = basicPerk.Icon;
            btnText.text = basicPerk.Title;
        }
#endif
    }
}