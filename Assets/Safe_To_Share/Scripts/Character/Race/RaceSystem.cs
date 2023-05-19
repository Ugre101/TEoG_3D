using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Character.Race.Races;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Character.Race
{
    public sealed class RaceSystem
    {
        public delegate void RaceChanged(BasicRace oldRace, BasicRace newRace);

        Dictionary<BasicRace, RaceEssence> dict;

        public RaceSystem() => SortRaces();

        public List<RaceEssence> AllRaceEssence { get; private set; } = new();
        Dictionary<BasicRace, RaceEssence> RaceDict => dict ??= AllRaceEssence.ToDictionary(r => r.Race);


        public BasicRace Race { get; private set; }
        public BasicRace SecRace { get; private set; }

        public event RaceChanged RaceChangedEvent;

        public event RaceChanged SecRaceChangedEvent;

        public void AddRace(BasicRace race, int raceEssAmount = 100)
        {
            if (RaceDict.TryGetValue(race, out var essence))
                essence.IncreaseAmount(raceEssAmount);
            else
            {
                AllRaceEssence.Add(new RaceEssence(race, raceEssAmount));
                dict = null;
            }

            SortRaces();
        }

        public void RemoveRace(BasicRace race, int raceEssAmountToRemove)
        {
            if (RaceDict.TryGetValue(race, out var essence))
                essence.DecreaseAmount(raceEssAmountToRemove);
            SortRaces();
        }

        void SortRaces()
        {
            if (AllRaceEssence.RemoveAll(r => r.Amount <= 0) != 0)
                dict = null;
            AllRaceEssence.Sort((r1, r2) => r2.Amount.CompareTo(r1.Amount));
            BasicRace oldRace = Race,
                oldSecRace = SecRace;
            Race = AllRaceEssence.Count > 0 ? AllRaceEssence[0].Race : null;
            SecRace = AllRaceEssence.Count > 1 ? AllRaceEssence[1].Race : Race;
            if (oldRace != null && oldRace != Race)
                RaceChangedEvent?.Invoke(oldRace, Race);
            if (oldSecRace != null && oldSecRace != SecRace)
                SecRaceChangedEvent?.Invoke(oldSecRace, SecRace);
        }

        public RacesSave Save() => new(AllRaceEssence);

        public IEnumerator Load(RacesSave load)
        {
            AllRaceEssence = new List<RaceEssence>();
            dict = null;
            foreach (RacesSave.RaceSave save in load.saves)
            {
                var op = Addressables.LoadAssetAsync<BasicRace>(save.raceGuid);
                yield return op;
                if (op.Status == AsyncOperationStatus.Succeeded)
                    AllRaceEssence.Add(new RaceEssence(op.Result, save.amount));
            }

            SortRaces();
        }
    }
}