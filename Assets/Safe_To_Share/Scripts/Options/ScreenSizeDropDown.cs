using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

namespace Options
{
    public class ScreenSizeDropDown : MonoBehaviour
    {
        [SerializeField] TMP_Dropdown dropdown;

        Resolution[] resolutions;

        void Start()
        {
            resolutions = Screen.resolutions;
            Setup();
        }

        public void ChangeScreenRes(int arg0)
        {
            if (arg0 < 0 || resolutions.Length < arg0) return;

            Resolution newRes = resolutions[arg0];
            Screen.SetResolution(newRes.width, newRes.height, Screen.fullScreenMode);
        }


        void Setup()
        {
            dropdown.ClearOptions();

            TMP_Dropdown.OptionDataList dataList = new();
            List<TMP_Dropdown.OptionData> options =
                resolutions.Select(res => new TMP_Dropdown.OptionData(res.ToString())).ToList();
            dataList.options.AddRange(options);
            dropdown.AddOptions(options);


            int current = Array.IndexOf(resolutions, Screen.currentResolution);
            if (current > -1)
                dropdown.value = current;
        }
    }
}