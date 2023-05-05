using System;
using Character.Organs;
using Safe_To_Share.Scripts.Static;

namespace Safe_To_Share.Scripts.Character.Organs.SexualOrgan
{
    [Serializable]
    public class Vagina : BaseOrgan
    {
        public override string OrganDesc(bool capitalLeter = true) => $"{(capitalLeter ? "A" : "a")} {Value.ConvertCm()} ";
        
    }
}