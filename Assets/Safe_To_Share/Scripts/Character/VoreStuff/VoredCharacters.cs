using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Character.VoreStuff
{
    public static class VoredCharacters
    {
        static List<Prey> preys = new();
        public static IReadOnlyDictionary<int, Prey> PreyDict = new Dictionary<int, Prey>();

        public static IEnumerable<Prey> GetPreys(IEnumerable<int> preyIds)
        {
            foreach (int id in preyIds)
                if (PreyDict.TryGetValue(id, out Prey prey))
                    yield return prey;
        }

        public static void AddPrey(Prey newPrey)
        {
            if (PreyDict.ContainsKey(newPrey.Identity.ID))
                return;
            preys.Add(newPrey);
            PreyDict = preys.ToDictionary(p => p.Identity.ID);
        }

        public static void RemovePrey(Prey prey)
        {
            preys.Remove(prey);
            PreyDict = preys.ToDictionary(p => p.Identity.ID);
        }

        public static float CurrentPreyTotalWeight(IEnumerable<int> ids) => GetPreys(ids).Sum(p => p.Body.Weight);

        public static VoredCharactersSave Save() => new(preys);

        public static IEnumerator Load(VoredCharactersSave toLoad)
        {
            preys = new List<Prey>();
            foreach (CharacterSave characterSave in toLoad.Preys)
            {
                Prey loaded = JsonUtility.FromJson<Prey>(characterSave.RawCharacter);
                yield return loaded.Load(characterSave);
                preys.Add(loaded);
            }

            PreyDict = preys.ToDictionary(p => p.Identity.ID);
        }
    }

    [Serializable]
    public struct VoredCharactersSave
    {
        [SerializeField] List<CharacterSave> preySaves;

        public VoredCharactersSave(IEnumerable<Prey> preys)
        {
            preySaves = new List<CharacterSave>();
            foreach (Prey prey in preys)
                preySaves.Add(new CharacterSave(prey));
        }

        public List<CharacterSave> Preys => preySaves;
    }
}