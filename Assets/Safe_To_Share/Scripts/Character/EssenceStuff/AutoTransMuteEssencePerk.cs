using System;
using UnityEngine;

namespace Character.EssenceStuff
{
    [CreateAssetMenu(menuName = "Character/Create AutoTransMuteEssencePerk", fileName = "AutoTransMuteEssencePerk", order = 0)]
    public class AutoTransMuteEssencePerk : EssencePerk
    {
        [SerializeField, Range(0, 1000f),] int essenceToChange = 25;
        [SerializeField, Range(0, 100f),] int bonus;

        public override void OnPartnerOrgasmPerkEffect(BaseCharacter perkOwner, BaseCharacter partner)
        {
            ChangeLog changeLog = new();
            switch (perkOwner.Essence.EssenceOptions.TransmuteTo)
            {
                case DrainEssenceType.None:
                    break;
                case DrainEssenceType.Masc:
                    partner.GainMasc(partner.LoseFemi(essenceToChange, changeLog) + bonus);
                    partner.InvokeUpdateAvatar();
                    break;
                case DrainEssenceType.Femi:
                    partner.GainFemi(partner.LoseMasc(essenceToChange, changeLog) + bonus);
                    partner.InvokeUpdateAvatar();
                    break;
                case DrainEssenceType.Both:
                    // ??!
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}