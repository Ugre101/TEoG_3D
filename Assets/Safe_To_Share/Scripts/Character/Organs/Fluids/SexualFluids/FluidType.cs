using System;

namespace Character.Organs.Fluids.SexualFluids
{
    [Serializable]
    public abstract class FluidType
    {
        public abstract string Title { get; }
        public abstract string Taste { get; }
        public abstract string Desc { get; }

        // Warning after build do not change fluid class Names
    }

    public sealed class ErrorFluid : FluidType
    {
        public override string Title => "ErrorFluid101";

        public override string Taste => "Like something is wrong";

        public override string Desc => "Something went wrong";
    }
}