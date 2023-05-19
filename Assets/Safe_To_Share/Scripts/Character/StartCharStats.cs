using Character.StatsStuff;
using UnityEngine;

namespace Safe_To_Share.Scripts.Character
{
    public sealed class StartCharStats : MonoBehaviour
    {
        [SerializeField] int startAmount = 20;
        [SerializeField] Stats stats = new(5, 5, 5, 5, 5);
        [SerializeField] StartCharStatSlider charStatSlider;
        [SerializeField] Transform content;

        void Start()
        {
            foreach (var (key, value) in stats.GetCharStats)
                Instantiate(charStatSlider, content).Setup(value, key);
        }
    }
}