using System;
using System.Collections.Generic;
using Items;
using UnityEngine;

namespace Safe_To_Share.Scripts.QuestStuff.Tasks {
    public sealed class QuestGatherTask : QuestBaseTask {
        public new const string Name = "GatherTask";
        [field: SerializeField] public List<NeedToGather> Needed { get; private set; } = new();

#if UNITY_EDITOR
        void OnValidate() {
            foreach (var needToGather in Needed)
                needToGather.Validate();
        }
#endif

        [Serializable]
        public class NeedToGather {
            [field: SerializeField]
            public string ItemGuid { get; private set; }

            [field: SerializeField, Range(1, 9),]
            public int Amount { get; private set; }
#if UNITY_EDITOR
            [field: SerializeField] public Item Item { get; private set; }
            public void Validate() {
                if (Item != null)
                    ItemGuid = Item.Guid;
            }
#endif
        }
    }
}