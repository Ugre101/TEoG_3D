using System.Collections.Generic;
using Safe_To_Share.Scripts.Static;
using UnityEngine;

namespace Safe_To_Share.Scripts.Holders {
    public sealed class EnemyObjectPool : MonoBehaviour {
        [SerializeField] EnemyAiHolder prefab;
        Queue<EnemyAiHolder> enemyHolders;

        void Awake() => Setup();

        void Setup() {
            enemyHolders = new Queue<EnemyAiHolder>(gameObject.GetComponentsInChildren<EnemyAiHolder>(true));
            transform.SleepChildren();
        }

        public EnemyAiHolder GetEnemyHolder() => enemyHolders.Count > 0 ? enemyHolders.Dequeue() : Instantiate(prefab);

        public void ReturnEnemyHolder(EnemyAiHolder aiHolder) {
            aiHolder.transform.SetParent(transform);
            enemyHolders.Enqueue(aiHolder);
            aiHolder.gameObject.SetActive(false);
        }
    }
}