using System;

namespace Character.Organs.Fluids.SexualFluids
{
    public class Milk : FluidType
    {
        public override string Title => "Milk";

        public override string Taste => "Creamy";

        public override string Desc => throw new NotImplementedException();
    }
}