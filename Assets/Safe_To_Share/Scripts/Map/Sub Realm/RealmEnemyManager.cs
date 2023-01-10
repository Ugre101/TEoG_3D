using System;
using System.Collections.Generic;
using Character.EnemyStuff;
using Safe_To_Share.Scripts.Holders;
using SaveStuff;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Safe_To_Share.Scripts.Map.Sub_Realm
{
    public class RealmEnemyManager : MonoBehaviour
    {
        [SerializeField] AssetReference enemyPrefab;
        [SerializeField] SubRealmSceneSo realmSceneSo;
        [SerializeField] List<Vector3> spawnPoints = new();
        void Start()
        {
            if (realmSceneSo.Enemies.ContainsKey(gameObject.name))
            {
                foreach (SubRealmSceneSo.SavedEnemy enemy in realmSceneSo.Enemies[gameObject.name])
                {
                    Addressables.InstantiateAsync(enemyPrefab).Completed += op =>
                    {
                        if (op.Result.TryGetComponent(out SubRealmEnemy realmEnemy))
                        {
                            realmEnemy.Setup(enemy.Enemy);
                            realmEnemy.transform.SetPositionAndRotation(enemy.Position, enemy.Rotation);
                        }
                    };
                }
            }
            else
            {
                Addressables.InstantiateAsync(enemyPrefab).Completed += op =>
                {
                    
                };
            }
        }
    }
    
}