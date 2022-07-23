using System;
using Safe_To_Share.Scripts.Static;
using UnityEngine;

namespace Character.IdentityStuff
{
    [Serializable]
    public class Identity
    {
        [SerializeField] int id;
        [SerializeField] string firstName, lastName;
        [SerializeField] BirthDay birthDay;

        public Identity()
        {
            id = IDGiver.NewID();
            birthDay = BirthDay.BirthedToday(21);
        }

        public Identity(string firstName, string lastName, BirthDay birthDay) // 18
        {
            this.firstName = firstName;
            this.lastName = lastName;
            this.birthDay = birthDay;
            id = IDGiver.NewID();
        }

        public int ID => id;

        public string FullName => $"{firstName} {lastName}";

        public string FirstName => firstName;

        public string LastName => lastName;

        public BirthDay BirthDay => birthDay;

        public void ChangeLastName(string newName) => lastName = newName;

        public void ChangeFirstName(string newName) => firstName = newName;
    }

    public static class IdentityExtensions
    {
        public static int DaysOld(this BirthDay birthDay)
        {
            int yearDays = (DateSystem.Year - birthDay.Year) * 365;
            int days = DateSystem.Day - birthDay.Day;
            return yearDays + days;
        }
    }
}