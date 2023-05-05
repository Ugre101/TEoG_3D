using Character.GenderStuff;
using Safe_To_Share.Scripts.Static;
using TMPro;
using UnityEngine;

namespace Options
{
    public class RenameAGender : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI orgGenderName;
        [SerializeField] TMP_InputField currentGenderName;
        [SerializeField] Gender gender;

        void Start()
        {
            currentGenderName.SetTextWithoutNotify(gender.GenderString());
            currentGenderName.onSubmit.AddListener(ChangeName);
        }

#if UNITY_EDITOR
        void OnValidate()
        {
            if (Application.isPlaying)
                return;
            if (orgGenderName != null)
                orgGenderName.text = UgreTools.StringFormatting.AddSpaceAfterCapitalLetter(nameof(gender));
        }
#endif


        void ChangeName(string arg0) => GenderSettings.ReNameGender(gender, arg0);
    }
}