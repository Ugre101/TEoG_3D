using System;
using System.Collections.Generic;
using System.Linq;
using Character.Organs.Fluids;
using UnityEngine;

namespace Character.PregnancyStuff {
    [Serializable]
    public class Womb {
        [SerializeField] List<Fetus> fetusList = new();
        [SerializeField] ForeignFluids foreignForeignFluids = new();
        public List<Fetus> FetusList => fetusList;

        public ForeignFluids ForeignFluids => foreignForeignFluids;
        public bool HasFetus => FetusList.Any();


        public IEnumerable<Fetus> GrowFetuses(int days = 1) {
            for (var i = FetusList.Count; i-- > 0;) {
                var fetus = FetusList[i];
                if (GrowAFetus(days, fetus))
                    yield return fetus;
            }
        }


        bool GrowAFetus(int days, Fetus fetus) {
            if (!fetus.GrowChild(days)) return false;
            FetusList.Remove(fetus);
            return true;
        }

        public void AddFetus(BaseCharacter mother, BaseCharacter father) => FetusList.Add(new Fetus(mother, father));
    }
}