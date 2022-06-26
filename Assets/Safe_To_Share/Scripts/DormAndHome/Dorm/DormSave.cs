using System;
using System.Collections.Generic;
using Character;
using DormAndHome.Dorm.Buildings;
using SaveStuff;
using UnityEngine;

namespace DormAndHome.Dorm
{
    [Serializable]
    public struct DormSave
    {
        [SerializeField] List<CharacterSave> mates;
        [SerializeField] string buildingsSave;

        public DormSave(List<DormMate> dormMates, DormBuildings buildingsSave)
        {
            this.buildingsSave = JsonUtility.ToJson(buildingsSave);
            mates = new List<CharacterSave>();
            foreach (DormMate mate in dormMates)
                mates.Add(new CharacterSave(mate));
        }

        public List<CharacterSave> Mates => mates;

        public string BuildingsSave => buildingsSave;
    }
}