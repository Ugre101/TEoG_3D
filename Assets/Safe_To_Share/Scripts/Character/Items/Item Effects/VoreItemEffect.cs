using System;
using Character;
using Items;
using UnityEngine;

namespace Safe_To_Share.Scripts.Character.Items.Item_Effects
{
    [Serializable]
    public class VoreItemEffect : ItemEffect
    {
        [SerializeField] AssignTempMod digestionMods;
        [SerializeField] AssignTempMod capacityMods;
        [SerializeField] AssignTempMod pleasureMods;
        [SerializeField] AssignTempMod pleasureDrainModes;

        public override void OnUse(BaseCharacter user, string itemGuid)
        {
            digestionMods.AddMods(user.Vore.digestionStrength.Mods, itemGuid);
            capacityMods.AddMods(user.Vore.capacityBoost, itemGuid);
            pleasureMods.AddMods(user.Vore.pleasureDigestion.Mods, itemGuid);
            pleasureDrainModes.AddMods(user.Vore.orgasmDrain.Mods, itemGuid);
        }
    }
}