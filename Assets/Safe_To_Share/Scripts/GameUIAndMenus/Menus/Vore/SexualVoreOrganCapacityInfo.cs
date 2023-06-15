﻿using Character;
using Character.Organs;
using Character.VoreStuff;

namespace Safe_To_Share.Scripts.GameUIAndMenus.Menus.Vore {
    public sealed class SexualVoreOrganCapacityInfo : VoreOrganCapacityInfo {
        public void Setup(BaseCharacter pred, SexualOrganType type, BaseOrgan organ) {
            var capacity = VoreSystemExtension.OrganVoreCapacity(pred, organ, type);
            Setup(nameof(type), organ.Vore, capacity);
        }
    }
}