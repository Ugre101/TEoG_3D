using System.Collections.Generic;
using Character;
using UnityEngine.AddressableAssets;

namespace Safe_To_Share.Scripts.Farming
{
    public class FarmArea : ITickHour
    {
        public FarmArea(string sceneGuid)
        {
            this.SceneGuid = sceneGuid;
        }
        public string SceneGuid;
        public List<PlantStats> Plants = new List<PlantStats>();
        public bool TickHour(int ticks = 1)
        {
            foreach (PlantStats plantStats in Plants)
            {
                
            }

            return false;
        }

        public void Load()
        {
        }

        public void AddPlant(PlantStats stats)
        {
            Plants.Add(stats);
        }
    }
}