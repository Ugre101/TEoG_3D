using System;
using Character;
using Dialogue.DialogueActions;
using Safe_To_Share.Scripts.Map;
using SceneStuff;
using UnityEngine;

namespace Safe_To_Share.Scripts.Dialogue.Events {
    [Serializable]
    public class SpotIsland : DialogueBaseAction {
        [SerializeField] LocationSceneSo sceneSo;
        [SerializeField] SceneTeleportExit exit;
        public override bool MeetsCondition() => KnowLocationsManager.Dict.ContainsKey(sceneSo.Guid) is false;

        public override void Invoke(BaseCharacter toAdd) {
            KnowLocationsManager.LearnLocation(sceneSo,exit.Guid);
        }
    }
}