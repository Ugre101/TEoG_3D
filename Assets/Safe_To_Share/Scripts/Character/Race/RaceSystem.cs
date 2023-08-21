﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Character.Race.Races;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Character.Race {
    public sealed class RaceSystem {
        public delegate void RaceChanged(BasicRace oldRace, BasicRace newRace);

        Dictionary<BasicRace, RaceEssence> dict;

        public RaceSystem() => SortRaces(true)
        ;

        /// <summary>
        /// A sorted list of all a characters race essences
        /// </summary>
        public List<RaceEssence> AllRaceEssence { get; private set; } = new();
        Dictionary<BasicRace, RaceEssence> RaceDict => dict ??= AllRaceEssence.ToDictionary(r => r.Race);


        public BasicRace Race { get; private set; }
        public BasicRace SecRace { get; private set; }

        public event RaceChanged RaceChangedEvent;

        public event RaceChanged SecRaceChangedEvent;

         int TransferEssence(BasicRace target, int amount) {
            if (!dict.TryGetValue(target, out var ess))
                return 0;
            int have = ess.Amount;
            ess.DecreaseAmount(amount);
            SortRaces(false);
            return Mathf.Min(amount, have);
        }

        public void DrainRaceEssence(RaceSystem from, BasicRace target, int amount) {
            var transferEssence = from.TransferEssence(target,amount);
            if (transferEssence <= 0)
                return;
            AddRace(target,transferEssence); 
            SortRaces(false);

        }
        public void AddRace(BasicRace race, int raceEssAmount = 100) {
            if (RaceDict.TryGetValue(race, out var essence)) {
                essence.IncreaseAmount(raceEssAmount);
            } else {
                AllRaceEssence.Add(new RaceEssence(race, raceEssAmount));
                dict = null;
            }

            SortRaces(false);
        }

        public void RemoveRace(BasicRace race) {
            if (RaceDict.TryGetValue(race, out var essence))
                essence.DecreaseAmount(essence.Amount + 1);
            SortRaces(false);
        } 
        public void RemoveAmountFromRace(BasicRace race, int raceEssAmountToRemove) {
            if (RaceDict.TryGetValue(race, out var essence))
                essence.DecreaseAmount(raceEssAmountToRemove);
            SortRaces(false);
        }

        void SortRaces(bool skipEvent) {
            if (AllRaceEssence.RemoveAll(r => r.Amount <= 0) != 0)
                dict = null;
            AllRaceEssence.Sort((r1, r2) => r2.Amount.CompareTo(r1.Amount));
            BasicRace oldRace = Race,
                      oldSecRace = SecRace;
            Race = AllRaceEssence.Count > 0 ? AllRaceEssence[0].Race : null;
            SecRace = AllRaceEssence.Count > 1 ? AllRaceEssence[1].Race : Race;
            if (skipEvent)
                return;
            if ((Race != null && oldRace == null) || (oldRace != null && oldRace != Race))
                RaceChangedEvent?.Invoke(oldRace, Race);
            if ((SecRace != null && oldSecRace == null) || (oldSecRace != null && oldSecRace != SecRace))
                SecRaceChangedEvent?.Invoke(oldSecRace, SecRace);
        }

        public RacesSave Save() => new(AllRaceEssence);

        public IEnumerator Load(RacesSave load) {
            AllRaceEssence = new List<RaceEssence>();
            dict = null;
            foreach (var save in load.saves) {
                var op = Addressables.LoadAssetAsync<BasicRace>(save.raceGuid);
                yield return op;
                if (op.Status == AsyncOperationStatus.Succeeded)
                    AllRaceEssence.Add(new RaceEssence(op.Result, save.amount));
            }

            SortRaces(true);
        }
    }
}