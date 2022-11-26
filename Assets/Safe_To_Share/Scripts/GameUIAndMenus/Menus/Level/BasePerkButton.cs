using AvatarStuff.Holders;
using Character.LevelStuff;
using Safe_To_Share.Scripts.Holders;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace GameUIAndMenus.Menus.Level
{
    public abstract class BasePerkButton : BaseLevelButton
    {
        [SerializeField, HideInInspector,] protected string guid;
        protected bool Have;
        protected BasicPerk loaded;

        void LoadedPerk(AsyncOperationHandle<BasicPerk> obj)
        {
            loaded = obj.Result;
            CanAfford(player.Player.LevelSystem.Points);
            HasPerk(obj.Result);
        }

        public override void Setup(PlayerHolder player)
        {
            this.player = player;
            Addressables.LoadAssetAsync<BasicPerk>(guid).Completed += LoadedPerk;
        }


        protected override void CanAfford(int obj) => Afford = loaded != null && loaded.Cost <= obj;

        protected void HasPerk(BasicPerk result)
        {
            bool hasPerk = player.Player.LevelSystem.OwnedPerks.Contains(result);
            HaveFade(hasPerk);
        }

        protected void HaveFade(bool value)
        {
            Have = value;
            Color color = rune.color;
            color.a = GetAplha(value);
            rune.color = color;
        }

        float GetAplha(bool havePerk) => havePerk ? 1f : loaded.MeetsRequirements(player.Player) ? 0.4f : 0.15f;
#if UNITY_EDITOR
        [SerializeField] BasicPerk basicPerk;

        void OnValidate()
        {
            guid = basicPerk != null ? basicPerk.Guid : string.Empty;
            if (rune == null || basicPerk == null)
                return;
            rune.sprite = basicPerk.Icon;
            btnText.text = basicPerk.Title;
        }
#endif
    }
}