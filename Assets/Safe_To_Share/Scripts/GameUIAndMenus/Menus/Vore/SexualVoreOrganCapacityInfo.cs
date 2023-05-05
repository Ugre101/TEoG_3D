using Character;
using Character.Organs;
using Character.VoreStuff;

namespace GameUIAndMenus.Menus.Vore
{
    public class SexualVoreOrganCapacityInfo : VoreOrganCapacityInfo
    {
        public void Setup(BaseCharacter pred, SexualOrganType type, BaseOrgan organ)
        {
            float capacity = VoreSystemExtension.OrganVoreCapacity(pred, organ, type);
            Setup(nameof(type), organ.Vore, capacity);
        }
    }
}