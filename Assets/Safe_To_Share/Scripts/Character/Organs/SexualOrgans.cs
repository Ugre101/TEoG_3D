using System;
using System.Collections.Generic;
using System.Linq;
using Character.Organs.OrgansContainers;
using UnityEngine;

namespace Character.Organs
{
    [Serializable]
    public class SexualOrgans : ITickMinute, ITickHour
    {
        [SerializeField] AnalsContainer anals = new();
        [SerializeField] DicksContainer dicks = new();
        [SerializeField] BallsContainer balls = new();
        [SerializeField] BoobsContainer boobs = new();
        [SerializeField] VaginaContainer vaginas = new();

        Dictionary<SexualOrganType, OrgansContainer> organsContainers;
        public DicksContainer Dicks => dicks;
        public BallsContainer Balls => balls;
        public BoobsContainer Boobs => boobs;
        public VaginaContainer Vaginas => vaginas;
        public AnalsContainer Anals => anals;
        public Dictionary<SexualOrganType, OrgansContainer> Containers => organsContainers ??=
            new Dictionary<SexualOrganType, OrgansContainer>
            {
                { SexualOrganType.Anal, anals },
                { SexualOrganType.Balls, balls },
                { SexualOrganType.Boobs, boobs },
                { SexualOrganType.Dick, dicks },
                { SexualOrganType.Vagina, vaginas },
            };

        public bool TickHour(int ticks = 1)
        {
            bool change = false;
            foreach (OrgansContainer container in Containers.Values)
                if (container.TickHour(ticks))
                    change = true;
            return change;
        }


        public void TickMin(int ticks = 1)
        {
            foreach (OrgansContainer container in Containers.Values)
                container.Fluid.TickMin(ticks);
        }

        public void Loaded()
        {
            if (Vaginas.List.Any(v => v.Womb.HasFetus)) Boobs.StartLactating();
        }
    }
}