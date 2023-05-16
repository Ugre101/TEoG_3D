using System;
using Character;
using Character.EssenceStuff.UI;
using Character.GenderStuff;
using Character.Organs.Fluids.UI;
using Character.PregnancyStuff;
using Character.SexStatsStuff.UI;
using TMPro;
using UnityEngine;

namespace Safe_To_Share.Scripts.AfterBattle.UI
{
    public sealed class AfterBattleActorInfo : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI title;
        [SerializeField] TextMeshProUGUI race;
        [SerializeField] ArousalSlider arousal;
        [SerializeField] TextMeshProUGUI orgasmCounter;
        [SerializeField] EssenceBars essenceBars;
        [SerializeField] TextMeshProUGUI gender;
        [SerializeField] MoreAfterBattleActorInfo moreInfo;
        [SerializeField] TextMeshProUGUI pregnant;
        [SerializeField] FluidInfos fluidInfos;
        [SerializeField] AfterBattleEssenceSliders afterBattleEssenceSliders;
        BaseCharacter actor;


        public void Setup(BaseCharacter character)
        {
            actor = character;
            title.text = character.Identity.FullName;
            if (character.RaceSystem.Race != null)
                race.text = character.RaceSystem.Race.Title;
            arousal.Setup(character.SexStats);
            character.SexStats.OrgasmChange += ChangeOrgasmText;
            essenceBars.Setup(character.Essence);
            gender.text = character.Gender.GenderString();
            moreInfo.Setup(character);
            pregnant.text = character.IsPregnant() ? "P" : string.Empty;
            fluidInfos.Setup(character);
            afterBattleEssenceSliders.Setup(character);
        }

        void OnDestroy()
        {
            actor.SexStats.OrgasmChange -= ChangeOrgasmText;
        }

        void ChangeOrgasmText(int obj) => orgasmCounter.text = $"Orgasms: {obj}";
    }
}