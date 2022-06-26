using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

namespace Options
{
    public class GraphicsTierDropDown : MonoBehaviour
    {
        TMP_Dropdown dropdown;
        // Start is called before the first frame update
        void Start()
        {
            if (!TryGetComponent(out TMP_Dropdown drop)) 
                return;
        
            dropdown = drop;
            Setup();
            dropdown.onValueChanged.AddListener(ChangeTier);
        }

        static void ChangeTier(int arg0)
        { 
            if (arg0 < 0 || arg0 >= QualitySettings.names.Length)
                return;
            QualitySettings.SetQualityLevel(arg0);
        }

        void Setup()
        {
            dropdown.ClearOptions();
            List<TMP_Dropdown.OptionData> qualities = QualitySettings.names.Select(q => new TMP_Dropdown.OptionData(q)).ToList();
            dropdown.AddOptions(qualities);
            dropdown.value = QualitySettings.GetQualityLevel();
        }
    }
}
