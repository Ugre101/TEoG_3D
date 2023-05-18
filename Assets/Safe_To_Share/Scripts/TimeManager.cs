using Safe_To_Share.Scripts.Static;
using UnityEngine;

namespace Safe_To_Share.Scripts
{
    public sealed class TimeManager : MonoBehaviour
    {
        const int TickMinuteEveryXSecond = 2;

        float lastTick;

        void Update()
        {
            if (lastTick + TickMinuteEveryXSecond > Time.time)
                return;
            lastTick = Time.time;
            DateSystem.PassMinute();
        }
    }
}