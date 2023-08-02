using Safe_To_Share.Scripts.CustomClasses;
using SaveStuff;
using UnityEngine;

namespace Safe_To_Share.Scripts.Map {
    public class TriggerConditionKnowIsland : TriggerCondition {
        [SerializeField] GameSceneSo island;
        public override bool ShouldTrigger() {
            return !KnowLocationsManager.Dict.ContainsKey(island.Guid);
        }
    }
}