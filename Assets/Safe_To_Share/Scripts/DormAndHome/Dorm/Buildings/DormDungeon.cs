using System;
using System.Collections.Generic;
using AvatarStuff.Holders;

namespace DormAndHome.Dorm.Buildings
{
    [Serializable]
    public class DormDungeon : Building
    {
        protected override int[] UpgradeCosts => new[] { 250, 500, 1000, };

        public override void TickBuildingEffect(List<DormMate> dormMates)
        {
            if (Level <= 0)
                return;
            foreach (var mate in dormMates)
                if (mate.SleepIn == DormMateSleepIn.Dungeon)
                    DungeonTick(mate);
        }

        void DungeonTick(DormMate mate)
        {
            float oldValue = mate.RelationsShips.GetRelationShipWith(PlayerHolder.PlayerID).Submission;
            mate.RelationsShips.IncreaseSubmissivenessTowards(PlayerHolder.PlayerID, 1f);
            float newValue = mate.RelationsShips.GetRelationShipWith(PlayerHolder.PlayerID).Submission;
            if (oldValue < 10 && newValue >= 10)
            {
                // Reached thressHold
            }
            else if (oldValue < 25 && newValue >= 25)
            {
                // Same
            }
            else if (oldValue < 50 && newValue >= 50)
            {
                // Same
            }
        }
    }
}