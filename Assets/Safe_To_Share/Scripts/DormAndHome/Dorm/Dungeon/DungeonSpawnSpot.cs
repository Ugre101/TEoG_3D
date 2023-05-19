using AvatarStuff.Holders;
using UnityEngine;

namespace DormAndHome.Dorm.Dungeon
{
    public sealed class DungeonSpawnSpot : MonoBehaviour
    {
        [SerializeField] DormMateAiHolder mate;
        public bool Empty { get; set; } = true;

        public void Setup(DormMate toAdd)
        {
            Empty = false;
            mate.AddMate(toAdd);
            mate.gameObject.SetActive(true);
        }

        public void Clear()
        {
            mate.gameObject.SetActive(false);
            Empty = true;
        }
    }
}