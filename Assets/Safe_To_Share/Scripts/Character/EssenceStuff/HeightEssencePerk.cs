using System.Collections.Generic;
using Character.BodyStuff;
using Character.StatsStuff.Mods;
using UnityEngine;

namespace Character.EssenceStuff
{
    [CreateAssetMenu(fileName = "Create Height Change", menuName = "Character/Essence/Essence Height Perk")]
    public class HeightEssencePerk : EssencePerk
    {
        [SerializeField] List<IntMod> heightDrain = new();
        [SerializeField] List<IntMod> heightGive = new();

        public override void PerkDrainEssenceEffect(BaseCharacter perkOwner, BaseCharacter partner)
        {
            foreach (IntMod intMod in heightDrain)
            {
                float toSteal = intMod.ModType == ModType.Percent
                    ? partner.Body.Height.BaseValue * (intMod.ModValue / 100f)
                    : intMod.ModValue;
                if (partner.Body.Height.BaseValue - 1 < toSteal)
                {
                    toSteal = partner.Body.Height.BaseValue - 1;
                    if (toSteal <= 0)
                        return;
                }

                partner.Body.ShrinkBody(toSteal);
                perkOwner.Body.GrowBody(toSteal);
            }
        }

        public override void PerkGiveEssenceEffect(BaseCharacter perkOwner, BaseCharacter partner)
        {
            foreach (IntMod intMod in heightGive)
            {
                if (!perkOwner.Essence.EssenceOptions.GiveHeight)
                    return;
                float toGive = intMod.ModType == ModType.Percent
                    ? perkOwner.Body.Height.BaseValue * (intMod.ModValue / 100f)
                    : intMod.ModValue;
                if (perkOwner.Body.Height.BaseValue - 1 < toGive)
                {
                    toGive = perkOwner.Body.Height.BaseValue - 1f;
                    if (toGive <= 0)
                        return;
                }

                partner.Body.GrowBody(toGive);
                perkOwner.Body.ShrinkBody(toGive);
            }
        }
    }
}