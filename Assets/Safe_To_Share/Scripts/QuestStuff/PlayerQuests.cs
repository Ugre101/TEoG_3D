using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace QuestStuff
{
    public static class PlayerQuests
    {
        public static event Action<QuestInfo> QuestAdded;
        public static event Action<QuestInfo> QuestRemoved;

        public static event Action<QuestReward> QuestReward;
        static Dictionary<QuestInfo, QuestProgress> PrivateQuests { get; set; } = new();

        public static IReadOnlyDictionary<QuestInfo, QuestProgress> PrintQuests { get; private set; } = PrivateQuests;
        static List<QuestReturnInfo> PrivateCompletedQuests { get; set; } = new();

        public static ReadOnlyCollection<QuestReturnInfo> CompletedQuests { get; private set; }

        public static bool HasQuest(QuestInfo info) => PrivateQuests.ContainsKey(info);

        public static IEnumerator ProgressQuest(QuestInfo info, int progress = 1)
        {
            if (PrivateQuests.TryGetValue(info, out QuestProgress questProgress) &&
                questProgress.ProgressQuest(progress))
                yield return CompleteQuest(info);
        }

        public static IEnumerator CompleteQuest(QuestInfo info)
        {
            if (!HasQuest(info))
                yield break;
            if (info.HasInstantReward) 
                QuestReward?.Invoke(info.QuestReward);
            yield return info.QuestChain ? LoadNextQuest(info.NextGuid) : LoadReturnTo(info.ReturnGuid);
            RemoveQuest(info);
        }

        static IEnumerator LoadReturnTo(string guid)
        {
            var op = Addressables.LoadAssetAsync<QuestReturnInfo>(guid);
            yield return op;
            if (op.Status != AsyncOperationStatus.Succeeded)
                yield break;
            PrivateCompletedQuests.Add(op.Result);
            CompletedQuests = PrivateCompletedQuests.AsReadOnly();
            PrintQuests = PrivateQuests;
        }

        static IEnumerator LoadNextQuest(string guid)
        {
            var op = Addressables.LoadAssetAsync<QuestInfo>(guid);
            yield return op;
            if (op.Status != AsyncOperationStatus.Succeeded)
                yield break;
            AddQuest(op.Result);
            PrintQuests = PrivateQuests;
        }

        public static void AddQuest(QuestInfo info)
        {
            if (HasQuest(info))
                return;
            PrivateQuests.Add(info, new QuestProgress(info));
            PrintQuests = PrivateQuests;
            QuestAdded?.Invoke(info);
        }


        public static void RemoveQuest(QuestInfo info)
        {
            if (!HasQuest(info))
                return;
            PrivateQuests.Remove(info);
            PrintQuests = PrivateQuests;
            QuestRemoved?.Invoke(info);
        }

        public static QuestsSave Save()
            => new(PrivateQuests.Values,
                PrivateCompletedQuests.Select(returnInfo => returnInfo.Guid).ToList());

        public static IEnumerator Load(QuestsSave toLoad)
        {
            yield return LoadQuestProgress(toLoad);
            yield return LoadCompledtedQuests(toLoad);
        }
        static IEnumerator LoadQuestProgress(QuestsSave toLoad)
        {
            if (toLoad.Quests == null)
                yield break;
            PrivateQuests = new Dictionary<QuestInfo, QuestProgress>();
            foreach (QuestProgress loadQuest in toLoad.Quests)
            {
                var info = Addressables.LoadAssetAsync<QuestInfo>(loadQuest.QuestId);
                yield return info;
                if (NotSafe(info)) 
                    continue;
                PrivateQuests.Add(info.Result, new QuestProgress(info.Result, loadQuest.Progress));
                QuestAdded?.Invoke(info.Result);
            }
            PrintQuests = PrivateQuests;
        }

        static bool NotSafe(AsyncOperationHandle<QuestInfo> info) => info.Status != AsyncOperationStatus.Succeeded || PrivateQuests.ContainsKey(info.Result);

        static IEnumerator LoadCompledtedQuests(QuestsSave toLoad)
        {
            if (toLoad.Completed == null)
                yield break;
            PrivateCompletedQuests = new List<QuestReturnInfo>();
            foreach (AsyncOperationHandle<QuestReturnInfo> info in toLoad.Completed.Select(Addressables.LoadAssetAsync<QuestReturnInfo>))
            {
                yield return info;
                if (info.Status == AsyncOperationStatus.Succeeded)
                    PrivateCompletedQuests.Add(info.Result);
            }
            CompletedQuests = PrivateCompletedQuests.AsReadOnly();
        }
    }
    [Serializable]
    public struct QuestsSave
    {
        [SerializeField] List<QuestProgress> quests;
        [SerializeField] List<string> completed;

        public QuestsSave(IEnumerable<QuestProgress> questsArray, IEnumerable<string> completedGuids)
        {
            quests = new List<QuestProgress>();
            foreach (QuestProgress quest in questsArray)
                quests.Add(quest);
            completed = new List<string>();
            foreach (string doneGuid in completedGuids)
                Completed.Add(doneGuid);
        }

        public List<QuestProgress> Quests => quests;

        public List<string> Completed => completed;
    }
}