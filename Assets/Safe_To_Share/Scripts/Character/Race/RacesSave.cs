using System;
using System.Collections.Generic;

namespace Character.Race {
    [Serializable]
    public struct RacesSave {
        public List<RaceSave> saves;

        public RacesSave(IEnumerable<RaceEssence> races) {
            saves = new List<RaceSave>();
            foreach (var race in races)
                saves.Add(new RaceSave(race));
        }

        [Serializable]
        public struct RaceSave {
            public string raceGuid;
            public int amount;

            public RaceSave(RaceEssence raceEss) {
                raceGuid = raceEss.Race.Guid;
                amount = raceEss.Amount;
            }
        }
    }
}