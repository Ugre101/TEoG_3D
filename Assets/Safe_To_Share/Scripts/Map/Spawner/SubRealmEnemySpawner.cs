﻿using System;
using System.Collections;
using System.Collections.Generic;
using AvatarStuff.Holders;
using Character.CreateCharacterStuff;
using Character.EnemyStuff;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Map.Spawner
{
    public class SubRealmEnemySpawner : MonoBehaviour
    {
        [SerializeField] EnemyPreset[] enemyPresets;
        [SerializeField] AssetReference enemyPrefab;
        [SerializeField] List<Vector3> spawnPoints = new();
        [SerializeField] LayerMask[] spawnableLayers;

        EnemyAiHolder prefab;
        
        IEnumerator  Start()
        {
            var op = Addressables.LoadAssetAsync<GameObject>(enemyPrefab);
            yield return op;
            if (op.Result.TryGetComponent(out EnemyAiHolder enemy))
            {
                prefab = enemy;
                SpawnEnemies();
            }else Debug.LogError("Enemy prefab does not have Enemy component");
        }

        void SpawnEnemies()
        {
            
        }
    }
}