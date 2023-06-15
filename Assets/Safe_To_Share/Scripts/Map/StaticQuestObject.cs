using System;
using Map;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace QuestStuff {
    public sealed class StaticQuestObject : MiniMapBaseObject {
        [SerializeField] bool triggerComplete, triggerProgress;
        [SerializeField, HideInInspector,] string guid;

        bool loaded;
        QuestInfo loadedInfo;

        public bool HasQuest { get; private set; }

        void Start() => Addressables.LoadAssetAsync<QuestInfo>(guid).Completed += Loaded;

        void OnDestroy() {
            PlayerQuests.QuestAdded -= HandleAdded;
            PlayerQuests.QuestRemoved -= HandleRemoved;
        }

        void OnTriggerEnter(Collider other) {
            if (!loaded)
                return;
            if (!other.CompareTag("Player") || !PlayerQuests.HasQuest(loadedInfo))
                return;

            if (triggerComplete)
                StartCoroutine(PlayerQuests.CompleteQuest(loadedInfo));
            if (triggerProgress)
                StartCoroutine(PlayerQuests.ProgressQuest(loadedInfo));
        }

        public static event Action<StaticQuestObject> PrintMe;
        public static event Action<StaticQuestObject> RemoveMe;

        void Loaded(AsyncOperationHandle<QuestInfo> obj) {
            loaded = true;
            loadedInfo = obj.Result;
            PlayerQuests.QuestAdded += HandleAdded;
            PlayerQuests.QuestRemoved += HandleRemoved;
            if (PlayerQuests.HasQuest(loadedInfo)) {
                PrintMe?.Invoke(this);
                HasQuest = true;
            }
        }

        void HandleRemoved(QuestInfo obj) {
            if (!obj == loadedInfo)
                return;
            RemoveMe?.Invoke(this);
            HasQuest = false;
        }

        void HandleAdded(QuestInfo obj) {
            if (!obj == loadedInfo)
                return;
            PrintMe?.Invoke(this);
            HasQuest = true;
        }
#if UNITY_EDITOR
        [SerializeField] QuestInfo questInfo;

        void OnValidate() {
            if (Application.isPlaying)
                return;
            guid = questInfo.Guid;
            icon = questInfo.Icon;
        }
#endif
    }
}