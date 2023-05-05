using System;
using Character.Organs;
using Safe_To_Share.Scripts.Static;
using UnityEngine;

namespace Safe_To_Share.Scripts.Character.Organs.SexualOrgan
{
    [Serializable]
    public class Anal : BaseOrgan
    {
        public override string OrganDesc(bool capitalLeter = true) => $"{(capitalLeter ? "A" : "a")} {Value.ConvertCm()} long dick";
    }
}