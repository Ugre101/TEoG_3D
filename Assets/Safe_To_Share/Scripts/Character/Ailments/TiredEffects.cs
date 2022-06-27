using Character.PlayerStuff;
using Safe_To_Share.Scripts.Static;

namespace Character.Ailments
{
    public static class TiredEffects
    {
        static readonly Tired Tired = new();
        static readonly DeadTired DeadTired = new();

        public static bool CheckTired(this Player player)
        {
            int hoursSinceLastSlept = DateSystem.DateSaveHoursAgo(player.LastTimeSlept);
            if (hoursSinceLastSlept > 96) // 4 days
                return DeadTired.Gain(player) | Tired.Cure(player);
            if (hoursSinceLastSlept > 48) // 2 days
                return DeadTired.Cure(player) | Tired.Gain(player);
            // Not tired
            return DeadTired.Cure(player) | Tired.Cure(player);
        }
    }
}