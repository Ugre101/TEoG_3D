using System;
using Character.Organs;

namespace Character.VoreStuff {
    public static class VoreDigestionModeExtension {
        public static VoreType OrganToVoreType(this SexualOrganType organType) =>
            organType switch {
                SexualOrganType.Dick => VoreType.Cock,
                SexualOrganType.Balls => VoreType.Balls,
                SexualOrganType.Boobs => VoreType.Breast,
                SexualOrganType.Vagina => VoreType.UnBirth,
                SexualOrganType.Anal => VoreType.Anal,
                _ => throw new ArgumentOutOfRangeException(),
            };
    }
}