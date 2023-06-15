using System;
using Assets.Scripts.Character.EssenceStuff.Perks;
using UnityEngine;

namespace Character.EssenceStuff {
    [Serializable]
    public class EssenceOptions {
        [SerializeField] bool selfDrain;
        [SerializeField] bool giveHeight = true;
        [SerializeField] DrainEssenceType transmuteTo = DrainEssenceType.Femi;
        [SerializeField] GenderMorph.MorphToGender morphPartnerToGender;

        public bool SelfDrain {
            get => selfDrain;
            set => selfDrain = value;
        }

        public bool GiveHeight {
            get => giveHeight;
            set => giveHeight = value;
        }

        public DrainEssenceType TransmuteTo {
            get => transmuteTo;
            set => transmuteTo = value;
        }

        public GenderMorph.MorphToGender MorphPartnerToGender {
            get => morphPartnerToGender;
            set => morphPartnerToGender = value;
        }
    }
}