using System;
using Character;
using Character.IdentityStuff;
using Safe_To_Share.Scripts.Static;
using TMPro;
using UnityEngine;

namespace GameUIAndMenus.DialogueAndEventMenu
{
    public class SetFullName : MonoBehaviour
    {
        [SerializeField] TMP_InputField firstName, lastName;
        [SerializeField] TMP_Dropdown lastNameOption;

        Identity identity;

        public void SetFirst(string arg0) => identity.ChangeFirstName(arg0);

        public void SetLast(string arg0) => identity.ChangeLastName(arg0);


        public void Setup(BaseCharacter father,BaseCharacter mother, Identity toChange,bool birtEvent)
        {
            identity = toChange;
            if (birtEvent)
            {
                lastNameOption.gameObject.SetActive(true);
                lastNameOption.SetupTmpDropDown(LastNameOptions.Yours,ChangeDefault);
            }
            else
            {
                lastNameOption.gameObject.SetActive(false);
            }
        }
        void ChangeDefault(int arg0)
        {
            var res = UgreTools.IntToEnum(arg0, LastNameOptions.Yours);
            switch (res)
            {
                case LastNameOptions.Yours:
                    break;
                case LastNameOptions.Partners:
                    break;
                case LastNameOptions.Custom:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public Identity Done()
        {
            return identity;
        }
        
        public enum LastNameOptions
        {
            Yours,
            Partners,
            Custom,
        }

    }
}