using System;

namespace Character.BodyStuff.BodyBuild
{
    [Serializable]
    public class Thickset : BodyBuild
    {
        public Thickset(float baseValue) : base(baseValue)
        {
        }

        protected override float MinValue => -10f;
        protected override float MaxValue => 10f;

        protected override string Desc() =>
            Value switch
            {
                >= 10f => "Very thick frame",
                >= 5f => "Thick frame",
                <= -10 => "Very thin frame",
                <= -5f => "Thin frame",
                _ => "Normal frame",
            };
    }
}