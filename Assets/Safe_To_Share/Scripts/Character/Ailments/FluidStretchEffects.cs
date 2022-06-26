namespace Character.Ailments
{
    public static class FluidStretchEffects
    {
        static readonly FluidStretch fluidStretch = new();

        public static bool CheckFluidStretch(this BaseCharacter character)
        {
            foreach (var container in character.SexualOrgans.Containers.Values)
                if (container.Fluid.Value > 1f && container.Fluid.CurrentValue / container.Fluid.Value > 0.99f)
                {
                    fluidStretch.Gain(character);
                    return true;
                }

            fluidStretch.Cure(character);

            return false;
        }
    }
}