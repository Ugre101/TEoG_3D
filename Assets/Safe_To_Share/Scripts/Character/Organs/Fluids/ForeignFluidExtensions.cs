using System;
using System.Linq;
using System.Text;

namespace Character.Organs.Fluids {
    public static class ForeignFluidExtensions {
        public static string FluidOnBodyDesc(this BaseCharacter character) => "";

        public static string FluidInWomb(this BaseOrgan organ, SexualOrganType type) {
            if (organ.Womb.ForeignFluids.GetFluids == null || !organ.Womb.ForeignFluids.GetFluids.Any())
                return string.Empty;

            StringBuilder desc = new();
            var tot = organ.Womb.ForeignFluids.GetFluids.Sum(f => f.Amount);
            var sorted = organ.Womb.ForeignFluids.GetFluids.OrderByDescending(f => f.Amount).ToArray();
            if (sorted.Length == 0)
                return string.Empty;
            var biggestPer = organ.Womb.ForeignFluids.GetFluids.Max(f => f.Amount) / tot;
            if (sorted.Length == 1 || biggestPer > 0.9f) {
                desc.Append(sorted[0].FluidType);
            } else if (sorted.Length > 1 && biggestPer > 0.5f) {
                desc.Append($"{sorted[0].FluidType} with traces of  {sorted[1].FluidType}");
            } else {
                desc.Append(" a mix of ");
                if (sorted.Length > 0)
                    desc.Append(sorted[0].FluidType);
                if (sorted.Length > 1) {
                    desc.Append(sorted.Length < 3 ? " and " : ", ");
                    desc.Append(sorted[1].FluidType);
                }

                if (sorted.Length > 2)
                    desc.Append($" and {sorted[2].FluidType}");
            }

            switch (type) {
                case SexualOrganType.Dick:
                    break;
                case SexualOrganType.Balls:
                    break;
                case SexualOrganType.Boobs:
                    break;
                case SexualOrganType.Vagina:
                    return $"filled with {desc}";
                case SexualOrganType.Anal:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }

            return "";
        }

        public static void CleanAll(BaseCharacter character) {
            CleanBody(character);
            foreach (var container in character.SexualOrgans.Containers.Values)
            foreach (var organ in container.BaseList)
                organ.Womb.ForeignFluids.ClearFluids();
        }

        public static void CleanBody(BaseCharacter character) => character.SexStats.FluidsOnBody.ClearFluids();

        public static void CleanOrifices(BaseCharacter character, SexualOrganType toClean) {
            if (!character.SexualOrgans.Containers.TryGetValue(toClean, out var container)) return;
            foreach (var organ in container.BaseList)
                organ.Womb.ForeignFluids.ClearFluids();
        }
    }
}