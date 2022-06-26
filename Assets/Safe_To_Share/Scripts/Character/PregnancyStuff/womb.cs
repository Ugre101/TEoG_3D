using System;
using System.Collections.Generic;
using System.Linq;
using Character.Organs.Fluids;
using UnityEngine;

namespace Character.PregnancyStuff
{
    [Serializable]
    public class Womb
    {
        [SerializeField] List<Fetus> fetusList = new();
        [SerializeField] ForeignFluids foreignForeignFluids = new();
        public List<Fetus> FetusList => fetusList;

        public ForeignFluids ForeignFluids => foreignForeignFluids;
        public bool HasFetus => FetusList.Any();


        public void GrowFetuses(Action<Fetus> bornEvent, int days = 1)
        {
            for (int i = FetusList.Count; i-- > 0;)
                GrowAFetus(bornEvent, days, FetusList[i]);
        }

        void GrowAFetus(Action<Fetus> bornEvent, int days, Fetus fetus)
        {
            if (fetus.GrowChild(days))
            {
                FetusList.Remove(fetus);
                bornEvent?.Invoke(fetus);
            }
        }

        public void AddFetus(BaseCharacter mother, BaseCharacter father) => FetusList.Add(new Fetus(mother, father));
    }
}