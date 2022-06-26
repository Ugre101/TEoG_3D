using Character.GenderStuff;
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
            if (orgGenderName != null)
                orgGenderName.text = UgreTools.StringFormatting.AddSpaceAfterCapitalLetter(gender.ToString());
        }
#endif
        

        void ChangeName(string arg0) => GenderSettings.ReNameGender(gender, arg0);
    }
}