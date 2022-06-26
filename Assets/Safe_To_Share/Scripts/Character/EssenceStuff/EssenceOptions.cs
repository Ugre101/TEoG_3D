using Character.GenderStuff;
using System;
using UnityEngine;
using Assets.Scripts.Character.EssenceStuff.Perks;

namespace Character.EssenceStuff
{
    [Serializable]
    public class EssenceOptions
    {
        [SerializeField] bool selfDrain = false;
        [SerializeField] bool giveHeight = true;
        [SerializeField] DrainEssenceType transmuteTo = DrainEssenceType.Femi;
        [SerializeField] GenderMorph.MorphToGender morphPartnerToGender;

        public bool SelfDrain
        {
            get => selfDrain;
            set => selfDrain = value;
        }

        public bool GiveHeight
        {
            get => giveHeight;
            set => giveHeight = value;
        }

        public DrainEssenceType TransmuteTo
        {
            get => transmuteTo;
            set => transmuteTo = value;
        }
        public GenderMorph.MorphToGender MorphPartnerToGender 
        { 
            get => morphPartnerToGender; 
            set => morphPartnerToGender = value; 
        }
    }
}