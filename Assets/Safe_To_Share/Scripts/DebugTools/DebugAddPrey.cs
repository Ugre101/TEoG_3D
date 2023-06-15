using System;
using Character.CreateCharacterStuff;
using Safe_To_Share.Scripts.Holders;
using UnityEngine;

namespace Character.VoreStuff {
    public sealed class DebugAddPrey : MonoBehaviour {
        [SerializeField] CharacterPreset preset;

        [SerializeField] VoreType voreTarget;
        [SerializeField] PlayerHolder playerHolder;

        [ContextMenu("Add prey")]
        public void AddPreyToPlayer() {
            AddPrey();
        }

        async void AddPrey() {
            await preset.LoadAssets();
            var prey = new Prey(preset.NewCharacter());
            switch (voreTarget) {
                case VoreType.Oral:
                    if (playerHolder.Player.OralVore(prey) is false)
                        print("Could fit prey");
                    break;
                case VoreType.Balls:
                    break;
                case VoreType.UnBirth:
                    break;
                case VoreType.Anal:
                    break;
                case VoreType.Breast:
                    break;
                case VoreType.Cock:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}