using System.Collections;
using SaveStuff;
using SceneStuff;
using UnityEngine;

namespace Map
{
    public class UnLockableTriggerBoatMenu : TriggerBoatMenu
    {
        [SerializeField] LocationSceneSo altLocationSceneSo;
        [SerializeField] UnLockAble lockAble;
        bool UnLocked { get; set; }

        protected override void Start()
        {
            base.Start();
#if UNITY_EDITOR
            StartCoroutine(DelayedStart());
            IEnumerator DelayedStart()
            {
                yield return new WaitForSeconds(2f);
                CheckBoat();
            }
#else
            CheckBoat();
#endif
        }

        void CheckBoat()
        {
            if (!KnowLocationsManager.Dict.TryGetValue(SceneLoader.CurrentLocationSceneGuid, out var curLoc))
            {
                if (altLocationSceneSo == null)
                    gameObject.SetActive(false);
                return;
            }

            if (!curLoc.ExitsGuids.Contains(exit.Guid)) return;
            lockAble.UnLock();
            UnLocked = true;
        }

        protected override void OnPlayerEnter()
        {
            if (!UnLocked)
                UnLock();
            base.OnPlayerEnter();
        }

        private void UnLock()
        {
            lockAble.UnLock();
            UnLocked = true;
            if (altLocationSceneSo != null)
                KnowLocationsManager.LearnLocation(altLocationSceneSo, exit.Guid);
            else if (KnowLocationsManager.Dict.TryGetValue(SceneLoader.CurrentLocationSceneGuid, out var curLoc))
                curLoc.LearnExit(exit.Guid);
        }
    }
}