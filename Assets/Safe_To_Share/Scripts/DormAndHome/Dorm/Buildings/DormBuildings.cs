using System;
using UnityEngine;

namespace DormAndHome.Dorm.Buildings {
    [Serializable]
    public class DormBuildings {
        [SerializeField] DormLodge dormLodge = new();
        [SerializeField] DormGym gym = new();
        [SerializeField] DormKitchen kitchen = new();
        [SerializeField] DormDungeon dungeon = new();
        [SerializeField] DormVillageBuildings villageBuildings = new();
        public DormLodge DormLodge => dormLodge;

        public DormKitchen Kitchen => kitchen;

        public DormGym Gym => gym;

        public DormDungeon Dungeon => dungeon;

        public DormVillageBuildings VillageBuildings => villageBuildings;
    }
}