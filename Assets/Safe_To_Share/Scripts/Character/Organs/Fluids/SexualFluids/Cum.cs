using System;

namespace Character.Organs.Fluids.SexualFluids {
    public sealed class Cum : FluidType {
        public override string Title => "Cum";

        public override string Taste => "Salty";

        public override string Desc => throw new NotImplementedException();
    }
}