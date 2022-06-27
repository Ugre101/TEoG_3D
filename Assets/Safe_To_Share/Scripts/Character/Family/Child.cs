using System;
using System.Collections.Generic;
using Character.CreateCharacterStuff;
using Character.IdentityStuff;
using Safe_To_Share.Scripts.Static;
using UnityEngine;

namespace Character.Family
{
    [Serializable]
    public struct Child
    {
        [SerializeField] Identity identity;
        [SerializeField] FamilyTree familyTree;
        [SerializeField] List<string> raceGuids;

        public Child(string firstName, FamilyTree familyTree, List<string> raceGuids)
        {
            identity = new Identity(firstName, familyTree.Mother.LastName, BirthDay.BirthedToday());
            this.familyTree = familyTree;
            this.raceGuids = raceGuids;
        }
        public Identity Identity => identity;

        public FamilyTree FamilyTree => familyTree;

        public List<string> RaceGuids => raceGuids;

        public bool MyBirthDayToday()
        {
            if (DateSystem.Year == identity.BirthDay.Year)
                return false; // Literally birthday
            return DateSystem.Day == identity.BirthDay.Day;
        }
    }
}