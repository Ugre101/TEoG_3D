using System;
using System.Collections.Generic;
using QuestStuff;
using UnityEngine;

namespace Safe_To_Share.Scripts.GameEvents {
    [CreateAssetMenu(fileName = "Base Event", menuName = "Events/Base event", order = 0)]
    public sealed class GameBaseEvent : ScriptableObject {
        [SerializeField] string title;
        [SerializeField, TextArea,] string desc;
        [SerializeField] bool canReturnToStart;
        [SerializeField] List<QuestOption> subQuestOption = new();
        [SerializeField] List<EventOption> subOption = new();
    }


    [Serializable]
    public abstract class EventOption {
        [SerializeField] string title;
        [SerializeField] string desc;
        [SerializeField] List<EventOption> subOptions = new();

        public string Title => title;

        public string Desc => desc;

        public virtual void OptionEffect() { }
    }

    [Serializable]
    public class QuestOption : EventOption {
        [SerializeField] QuestInfo questInfo;
        public static event Action<QuestInfo> GainQuest;

        public override void OptionEffect() => GainQuest?.Invoke(questInfo);
    }

    public class TradeOption : EventOption { }
}