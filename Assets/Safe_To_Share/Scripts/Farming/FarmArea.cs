using System.Collections.Generic;
using Character;

namespace Safe_To_Share.Scripts.Farming
{
    public class FarmArea : ITickHour
    {
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
            Dictionary<string, Plant> plants = new Dictionary<string, Plant>();
            
        }
    }
}