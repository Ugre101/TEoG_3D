using Character.LevelStuff;
using UnityEngine;

namespace Safe_To_Share.Scripts.Holders
{
    public static class PerkExtension
    {
        public static void GainPerk(this BasicPerk perk, PlayerHolder playerHolder)
        {
            if (perk == null)
            {
                Debug.LogError("Perk is null");
                return;
            }

            if (playerHolder.Player.LevelSystem.OwnedPerks.Contains(perk))
            {
                Debug.LogError("Already owns perk");
                return;
            }

            switch (perk)
            {
                case MovementPerk movementPerk:
                    movementPerk.GainMovementMods(playerHolder);
                    break;
            }

            perk.PerkGainedEffect(playerHolder.Player);
            playerHolder.Player.LevelSystem.OwnedPerks.Add(perk);
        }
    }
}