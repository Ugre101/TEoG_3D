using System;
using System.Collections.Generic;
using System.Linq;
using Character;
using Character.IdentityStuff;
using Character.PregnancyStuff;
using Safe_To_Share.Scripts.GameUIAndMenus.BirthMenu;
using Safe_To_Share.Scripts.Static;
using TMPro;
using UnityEngine;

namespace Safe_To_Share.Scripts.GameUIAndMenus {
    public sealed class BirthEventMenu : MonoBehaviour {
        [SerializeField] TextMeshProUGUI title;

        [SerializeField] TextMeshProUGUI content;
        [SerializeField] GenderedNameList childNames;

        [SerializeField] Transform inputsContainer;
        [SerializeField] BabyNameInput babyNameInput;

        BabyNameInput[] added;
        Fetus[] born;
        BaseCharacter mother;
        string[] nameList;

        void OnDisable() {
            foreach (var nameInput in added)
                nameInput.NameChange -= NewBabyName;

            added = Array.Empty<BabyNameInput>();
            inputsContainer.KillChildren();
        }

        public void PlayerMotherBirthEvent(BaseCharacter mother, IEnumerable<Fetus> born) {
            this.mother = mother;
            var fetusEnumerable = born as Fetus[] ?? born.ToArray();
            this.born = fetusEnumerable;
            nameList = new string[fetusEnumerable.Length];
            for (var index = 0; index < nameList.Length; index++)
                nameList[index] = childNames.GetRandomNeutralName;
            title.text = "Birth";
            inputsContainer.KillChildren();
            added = new BabyNameInput[fetusEnumerable.Length];
            for (var i = 0; i < fetusEnumerable.Length; i++) {
                var imp = Instantiate(babyNameInput, inputsContainer);
                imp.Setup(i, nameList[i]);
                imp.NameChange += NewBabyName;
                added[i] = imp;
            }
        }

        void NewBabyName(string arg1, int arg2) => nameList[arg2] = arg1;

        public void GiveBirth() {
            for (var i = 0; i < born.Length; i++) {
                var fetus = born[i];
                mother.BaseOnBirth(fetus, nameList[i]);
            }
        }
    }
}