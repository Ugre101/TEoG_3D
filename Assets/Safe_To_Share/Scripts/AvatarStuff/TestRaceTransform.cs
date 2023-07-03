using System;
using AvatarStuff;
using Character.CreateCharacterStuff;
using Character.PlayerStuff;
using Character.Race.Races;
using UnityEngine;

namespace Safe_To_Share.Scripts.AvatarStuff {
    public class TestRaceTransform : MonoBehaviour {
        [SerializeField] CharacterPreset preset;
        [SerializeField] CharacterAvatar avatar;
        [SerializeField] RaceTransformation raceTransformation;


        [SerializeField] BasicRace orc;
        [SerializeField] BasicRace bovine;

        [SerializeField, Range(1, 200)] int toAdd = 50;

        [SerializeField] Player testPlayer;

        async void Start() {
           await preset.LoadAssets();
           testPlayer = new Player(preset.NewCharacter());
           print("loaded");
        }

        [ContextMenu("Add Orc")]
        public void AddOrc() {
            testPlayer.RaceSystem.AddRace(orc,toAdd);
        }
        
        [ContextMenu("Add bovine")]
        public void AddBovine() {
            testPlayer.RaceSystem.AddRace(bovine,toAdd);
        }

        [ContextMenu("Test")]
        public void Test() {
            foreach (var skinnedMeshRenderer in avatar.AllShapes)
                raceTransformation.Check(skinnedMeshRenderer, testPlayer);
        }

        
    }
}