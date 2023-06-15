using System;
using System.Collections.Generic;
using Character.Family;
using UnityEngine;

namespace Character.PregnancyStuff {
    [Serializable]
    public class Fetus {
        [SerializeField] FamilyTree familyTree;
        [SerializeField] int daysOld;
        [SerializeField] List<string> raceGuids;

        public Fetus(BaseCharacter mother, BaseCharacter father) {
            familyTree = new FamilyTree(father.Identity, mother.Identity);
            daysOld = 0;
            var races = new List<string>();
            if (mother.RaceSystem.Race != null)
                races.Add(mother.RaceSystem.Race.Guid);
            if (father.RaceSystem.Race != null)
                races.Add(father.RaceSystem.Race.Guid);
            raceGuids = races;
        }

        public int DaysOld => daysOld;

        public bool GrowChild(int days = 1) {
            daysOld += days;
            return PregnancyExtensions.IncubationDays <= DaysOld;
        }

        public Child GetBorn(string firstName) => new(firstName, familyTree, raceGuids);
    }
}