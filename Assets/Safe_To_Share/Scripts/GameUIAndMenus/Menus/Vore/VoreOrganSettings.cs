using System;
using System.Collections.Generic;
using System.Linq;
using Character.Organs;
using Character.PlayerStuff;
using Character.VoreStuff;
using Character.VoreStuff.VoreDigestionModes;
using Safe_To_Share.Scripts.Static;
using TMPro;
using UnityEngine;

namespace GameUIAndMenus.Menus.Vore
{
    public class VoreOrganSettings : MonoBehaviour
    {
        Player player;
        [SerializeField] GameObject noSelected, selected;
        [SerializeField] TextMeshProUGUI title;
        [SerializeField] TMP_Dropdown dropdown;

        void Start()
        {
            SexualVoreOrganContainerInfo.ShowOrganSettingForMe += ShowSexualOrganSettings;
            StomachVoreOrganContainerInfo.ShowStomachSettings += ShowOralSettings;
        }

        void OnEnable()
        {
            noSelected.gameObject.SetActive(true);
            selected.gameObject.SetActive(false);
        }

        void OnDisable()
        {
            dropdown.onValueChanged.RemoveAllListeners();
            dropdown.ClearOptions();
        }

        void OnDestroy()
        {
            SexualVoreOrganContainerInfo.ShowOrganSettingForMe -= ShowSexualOrganSettings;
            StomachVoreOrganContainerInfo.ShowStomachSettings -= ShowOralSettings;
        }

        public void Setup(Player player)
        {
            this.player = player;
        }
        void ShowOralSettings()
        {
            noSelected.gameObject.SetActive(false);
            selected.gameObject.SetActive(true);
            title.text = "Stomach";

            dropdown.ClearOptions();
            List<TMP_Dropdown.OptionData> options = player.Vore.StomachDigestionMode.GetPossibleDigestionTypes(player)
                .Select(digestionType => new TMP_Dropdown.OptionData(digestionType)).ToList();
            dropdown.AddOptions(options);
            dropdown.onValueChanged.RemoveAllListeners();
            dropdown.value = player.Vore.StomachDigestionMode.CurrentModeID;
            dropdown.onValueChanged.AddListener(NewMode);
            ShowPreys(VoredCharacters.GetPreys(player.Vore.Stomach.PreysIds), VoreType.Oral, player.Vore.StomachDigestionMode.CurrentModeTitle);
            
            void NewMode(int arg0)
            {
                StomachDigestionMode mode = player.Vore.StomachDigestionMode;
                int newMode = Array.IndexOf( mode.AllDigestionTypes, mode.GetPossibleDigestionTypes(player).ElementAt(arg0));
                player.Vore.StomachDigestionMode.SetDigestionMode(newMode);
            }
        }


        void ShowSexualOrganSettings(SexualOrganType type)
        {
            noSelected.gameObject.SetActive(false);
            selected.gameObject.SetActive(true);
            title.text = type.ToString();
            dropdown.ClearOptions();
            if (!player.Vore.VoreOrgans.TryGetValue(type, out var voreOrgan))
                return;
            List<TMP_Dropdown.OptionData> options = voreOrgan.GetPossibleDigestionTypes(player)
                .Select(t => new TMP_Dropdown.OptionData(t)).ToList();
            dropdown.AddOptions(options);
            dropdown.onValueChanged.RemoveAllListeners();
            dropdown.value = voreOrgan.CurrentModeID;
            dropdown.onValueChanged.AddListener(NewMode);
            if (!player.SexualOrgans.Containers.TryGetValue(type, out var container))
                return;
            List<Prey> allPreys = new();
            foreach (BaseOrgan baseOrgan in container.List)
            {
                allPreys.AddRange(VoredCharacters.GetPreys(baseOrgan.Vore.PreysIds));
                allPreys.AddRange(VoredCharacters.GetPreys(baseOrgan.Vore.SpecialPreysIds));
            }
            ShowPreys(allPreys, type.OrganToVoreType(), voreOrgan.CurrentModeTitle);

            void NewMode(int arg0)
            {
                int newMode = Array.IndexOf(voreOrgan.AllDigestionTypes, voreOrgan.GetPossibleDigestionTypes(player).ElementAt(arg0));
                voreOrgan.SetDigestionMode(newMode);
            }
        }
        [SerializeField] Transform preyContent;
        [SerializeField] PreyShowCase preyPrefab;
        void ShowPreys(IEnumerable<Prey> preys,VoreType voreType, string altMode)
        {
            preyContent.KillChildren();
            foreach (Prey prey in preys)
            {
              var preyPre =  Instantiate(preyPrefab,preyContent);
                  preyPre.Setup(prey, voreType, altMode);
                  preyPre.RegurgitateMe += Reg;
            }
        }

        void Reg(int arg1, VoreType arg2)
        {
            VoreSystemExtension.RegurgitatePrey(player,arg2,arg1);
        }
    }
}