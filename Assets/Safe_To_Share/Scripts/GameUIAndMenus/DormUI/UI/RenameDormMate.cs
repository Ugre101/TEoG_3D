using DormAndHome.Dorm;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Safe_To_Share.Scripts.GameUIAndMenus.DormUI.UI
{
    public class RenameDormMate : MonoBehaviour
    {
        [SerializeField] TMP_InputField firstName, lastName;
        [SerializeField] Button accept, close;
        [SerializeField] ViewSelectedDormMate selectedDormMate;

        DormMate mate;
        string tempFirst, tempLast;

        void Start()
        {
            close.onClick.AddListener(Close);
            firstName.onValueChanged.AddListener(FirstChange);
            lastName.onValueChanged.AddListener(LastChange);
        }



        void FirstChange(string arg0) => tempFirst = arg0;
        void LastChange(string arg0) => tempLast = arg0;

        void Close() => gameObject.SetActive(false);


        void ChangeName()
        {
            mate.Identity.ChangeFirstName(tempFirst);
            mate.Identity.ChangeLastName(tempLast);
            selectedDormMate.PrintName();
            selectedDormMate.RefreshSelectedButtonNames();
            gameObject.SetActive(false);
        }

        public void Setup(DormMate selectedMate)
        {
            gameObject.SetActive(true);
            mate = selectedMate;
            accept.onClick.RemoveAllListeners();
            accept.onClick.AddListener(ChangeName);
            tempFirst = mate.Identity.FirstName;
            firstName.text = tempFirst;
            tempLast = mate.Identity.LastName;
            lastName.text = tempLast;
        }
    }
}