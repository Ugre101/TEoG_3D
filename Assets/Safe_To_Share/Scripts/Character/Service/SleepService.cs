using Character.PlayerStuff;
using UnityEngine;

namespace Character.Service
{
    [CreateAssetMenu(fileName = "Create SleepService", menuName = "Services/Sleep Service", order = 0)]
    public class SleepService : BaseService
    {
        [SerializeField] int sleepQuality;

        public override void OnUse(Player player) => player.Sleep(sleepQuality);
    }
}