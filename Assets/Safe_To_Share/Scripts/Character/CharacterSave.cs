using System;
using Character.Race;
using CustomClasses;
using UnityEngine;

namespace Character {
    [Serializable]
    public struct CharacterSave {
        [SerializeField] string player;
        [SerializeField] SerializableScriptableObjectSaves perkSave;
        [SerializeField] SerializableScriptableObjectSaves essPerkSave;
        [SerializeField] SerializableScriptableObjectSaves vorePerkSave;
        [SerializeField] RacesSave racesSave;

        public CharacterSave(BaseCharacter player) {
            this.player = JsonUtility.ToJson(player);
            perkSave = player.LevelSystem.PerkSave();
            racesSave = player.RaceSystem.Save();
            essPerkSave = player.Essence.Save();
            vorePerkSave = player.Vore.Level.Save();
        }


        public SerializableScriptableObjectSaves PerkSave => perkSave;
        public RacesSave RacesSave => racesSave;

        public SerializableScriptableObjectSaves EssPerkSave => essPerkSave;

        public SerializableScriptableObjectSaves VorePerkSave => vorePerkSave;

        public string RawCharacter => player;
    }
}