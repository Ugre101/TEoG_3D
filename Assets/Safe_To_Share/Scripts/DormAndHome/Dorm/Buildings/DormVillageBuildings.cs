using System;
using UnityEngine;

namespace DormAndHome.Dorm.Buildings {
    [Serializable]
    public class DormVillageBuildings {
        [SerializeField] DormBrothel dormBrothel = new();

        public DormBrothel Brothel => dormBrothel;
    }
}