using System;
using Character.CreateCharacterStuff;
using UnityEngine;

namespace Safe_To_Share.Scripts.QuestStuff.Tasks
{
    public class QuestHuntTask : QuestBaseTask
    {
        public new const string Name = "HuntTask";
        [field: SerializeField] public string EnemyId { get; private set; }

#if UNITY_EDITOR
        [SerializeField] EnemyPreset enemyToHunt;
        void OnValidate()
        {
            if (enemyToHunt != null)
                EnemyId = enemyToHunt.EnemyGuid;
        }
#endif
    }
}