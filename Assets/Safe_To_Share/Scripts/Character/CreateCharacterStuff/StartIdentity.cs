using System;
using Character.GenderStuff;
using Character.IdentityStuff;
using Static;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Character.CreateCharacterStuff
{
    [Serializable]
    public class StartIdentity
    {
        [SerializeField] bool customIdentity;
        [SerializeField] Identity identity;
        [SerializeField] GenderedNameList nameList;
        [SerializeField, Range(18, 999),] int minAge = 18;
        [SerializeField, Range(18, 999),] int maxAge = 44;

        BirthDay RandomBirthDay => new(DateSystem.Year - Random.Range(minAge, maxAge), Random.Range(1, 365));

        public Identity GetIdentity(GenderType genderType) => customIdentity
            ? new Identity(identity.FirstName, identity.LastName, identity.BirthDay)
            : new Identity(GetName(genderType), GetRandomLastName(), RandomBirthDay);

        string GetRandomLastName() => nameList != null ? nameList.GetRandomLastName : "NoNameListSon";

        string GetName(GenderType type) =>
            nameList == null
                ? "NoNameList"
                : type switch
                {
                    GenderType.Neutral => nameList.GetRandomNeutralName,
                    GenderType.Feminine => nameList.GetRandomFemaleName,
                    GenderType.Masculine => nameList.GetRandomMaleName,
                    _ => throw new ArgumentOutOfRangeException(nameof(type), type, null),
                };
    }
}