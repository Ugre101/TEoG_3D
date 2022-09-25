using Character;
using Character.IdentityStuff;
using Character.PregnancyStuff;
using TMPro;
using UnityEngine;

namespace Safe_To_Share.Scripts.GameEvents
{
    public class BirthEventMenu : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI title;

        [SerializeField] TMP_InputField firstNameInput;
        [SerializeField] GenderedNameList childNames;
        Fetus born;
        string firstName;
        BaseCharacter mother;

        public void PlayerMotherBirthEvent(BaseCharacter mother, Fetus born)
        {
            this.mother = mother;
            this.born = born;
            firstNameInput.SetTextWithoutNotify(childNames.GetRandomNeutralName);
        }

        public void SetFirstName(string arg0)
        {
            firstName = arg0;
        }

        public void GiveBirth()
        {
            mother.BaseOnBirth(born, firstName);
        }
    }
}