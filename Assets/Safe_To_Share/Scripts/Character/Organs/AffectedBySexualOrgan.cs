using System;
using UnityEngine;

namespace Character.Organs
{
    [Serializable]
    public struct AffectedBySexualOrgan
    {
        [SerializeField] SexualOrganType sexualOrgan;
        [SerializeField] float modifer;

        public float Modifer => modifer;

        public SexualOrganType SexualOrgan => sexualOrgan;
    }
}