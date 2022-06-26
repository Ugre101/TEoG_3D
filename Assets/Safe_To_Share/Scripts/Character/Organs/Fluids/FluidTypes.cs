using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Reflection;
using Character.Organs.Fluids.SexualFluids;

namespace Character.Organs.Fluids
{
    public static class FluidTypes
    {
        
        
        static Dictionary<string, FluidType> fluidTypes;
        
        public static Dictionary<string, FluidType> FluidsDict
        {
            get
            {
                if (fluidTypes == null)
                    Init();
                return fluidTypes;
            }
        }

        public static FluidType GetFluid(string name) =>
            FluidsDict.ContainsKey(name) ? FluidsDict[name] : new ErrorFluid();

        public static FluidType GetFluid(FluidType fluidClass) => GetFluid(fluidClass.GetType().Name);

        static void Init()
        {
            var fluids = Assembly.GetAssembly(typeof(FluidType)).GetTypes()
                .Where(t => t.IsSubclassOf(typeof(FluidType)) && !t.IsAbstract && t.IsClass);

            fluidTypes = new Dictionary<string, FluidType>();
            foreach (Type type in fluids)
            {
                FluidType fluid = Activator.CreateInstance(type) as FluidType;

                FluidsDict.Add(type.Name, fluid);
            }
        }
    }
}