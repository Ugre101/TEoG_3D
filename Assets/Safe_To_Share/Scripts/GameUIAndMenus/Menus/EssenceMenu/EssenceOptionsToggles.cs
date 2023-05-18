using System.Linq;
using Assets.Scripts.Character.EssenceStuff.Perks;
using Character.EssenceStuff;
using Character.PlayerStuff;
using Safe_To_Share.Scripts.Static;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Safe_To_Share.Scripts.GameUIAndMenus.Menus.EssenceMenu
{
    public class EssenceOptionsToggles : MonoBehaviour
    {
        [SerializeField] Toggle selfDrain;
        [SerializeField] Toggle giveHeight;
        [SerializeField] TMP_Dropdown transmuteOption;
        [SerializeField] TMP_Dropdown genderMorhpOption;
        Player player;

        void Start()
        {
            selfDrain.onValueChanged.AddListener(ToggleSelfDrain);
            giveHeight.onValueChanged.AddListener(ToggleGiveHeight);
        }

        void ToggleSelfDrain(bool arg0) => player.Essence.EssenceOptions.SelfDrain = arg0;

        void ToggleGiveHeight(bool arg0) => player.Essence.EssenceOptions.GiveHeight = arg0;


        public void Setup(Player parPlayer)
        {
            player = parPlayer;
            SetupToggle(player.Essence.GiveAmount.Value > 0, selfDrain, player.Essence.EssenceOptions.SelfDrain);
            SetupToggle(player.Essence.EssencePerks.OfType<HeightEssencePerk>().Any(), giveHeight,
                player.Essence.EssenceOptions.GiveHeight);

            SetupTransmute();
            SetupGenderMorph();
        }

        void SetupGenderMorph()
        {
            bool hasGenderMorphPerk = player.Essence.EssencePerks.OfType<GenderMorph>().Any();
            if (hasGenderMorphPerk)
            {
                genderMorhpOption.SetupTmpDropDown(player.Essence.EssenceOptions.MorphPartnerToGender, SetMode);

                void SetMode(int arg) => player.Essence.EssenceOptions.MorphPartnerToGender =
                    UgreTools.IntToEnum(arg, GenderMorph.MorphToGender.Disabled);
            }

            genderMorhpOption.gameObject.SetActive(hasGenderMorphPerk);
        }

        void SetupTransmute()
        {
            bool hasTransMutePerk = player.Essence.EssencePerks.OfType<AutoTransMuteEssencePerk>().Any();
            if (hasTransMutePerk)
            {
                transmuteOption.SetupTmpDropDown(player.Essence.EssenceOptions.TransmuteTo, SetMode,
                    DrainEssenceType.Both);

                void SetMode(int arg) => player.Essence.EssenceOptions.TransmuteTo =
                    UgreTools.IntToEnum(arg, DrainEssenceType.None, DrainEssenceType.Both);
            }

            transmuteOption.gameObject.SetActive(hasTransMutePerk);
        }

        void SetupToggle(bool showToggle, Toggle toggle, bool isOn)
        {
            toggle.gameObject.SetActive(showToggle);
            if (!showToggle)
                return;
            toggle.isOn = isOn;
        }
    }
}