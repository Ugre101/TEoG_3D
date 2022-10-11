using Character;
using Character.Organs;
using Character.PlayerStuff;
using Items;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Safe_To_Share.Scripts.Special_Items
{
    [CreateAssetMenu(menuName = "Items/Special Items/Create Milker", fileName = "Breast Milker Kit", order = 0)]
    public class MilkerKitItem : Item
    {
        [SerializeField] AssetReference smallFlask;
        [SerializeField] AssetReference flask;
        [SerializeField] AssetReference largeFlask;

        [SerializeField] SexualOrganType milkOrgan; 
        public override void Use(BaseCharacter user)
        {
            if (user is not Player player) return;
            if(!user.SexualOrgans.Containers.TryGetValue(milkOrgan,out var container))
                return;
            float fluid = container.FluidCurrent;
            if (fluid > 1000)
            {
                int largeFlasks = Mathf.FloorToInt(fluid / 1000);
                player.Inventory.AddItem(largeFlask.AssetGUID, largeFlasks);
                fluid %= 1000;
            }

            if (fluid > 250)
            {
                int flasks = Mathf.FloorToInt(fluid / 250);
                player.Inventory.AddItem(flask.AssetGUID, flasks);
                fluid %= 250;
            }

            if (fluid > 50)
            {
                int flasks = Mathf.FloorToInt(fluid / 50);
                player.Inventory.AddItem(smallFlask.AssetGUID, flasks);
            }

            container.Fluid.SetEmpty();
        }
    }
}