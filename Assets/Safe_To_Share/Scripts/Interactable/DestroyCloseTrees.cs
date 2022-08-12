using System;
using System.Collections.Generic;
using AvatarStuff.Holders;
using Map;
using Safe_To_Share.Scripts.Helpers;
using UnityEngine;

namespace Safe_To_Share.Scripts.Interactable
{
    public class DestroyCloseTrees : MonoBehaviour
    {
        const int FrameLimit = 8;

        [SerializeField, Range(float.Epsilon, 5f),]
        float removeDist = 2f;

        [SerializeField, Range(2f, 5f),] float heightReq = 3f;
        bool foundCollider;
        bool hasDebrisPool;
        TreeInstance[] paryTrees;
        Vector3 pvecTerrainPosition;
        Vector3 pvecTerrainSize;
        TerrainCollider terrainCollider;

        void Start()
        {
            if (Terrain.activeTerrain == null)
            {
                enabled = false;
                return;
            }

            PlayerPosition.PlayerMoved += Tick; 
            var activeTerrain = Terrain.activeTerrain;
            pvecTerrainPosition = activeTerrain.transform.position;
            var terrainData = activeTerrain.terrainData;
            pvecTerrainSize = terrainData.size;
            paryTrees = terrainData.treeInstances;
            hasDebrisPool = TreeDebrisObjectPool.Instance != null;
            if (!Terrain.activeTerrain.TryGetComponent(out TerrainCollider component)) return;
            foundCollider = true;
            terrainCollider = component;
#if UNITY_EDITOR
            orgTrees = paryTrees;
#endif
        }

        void OnDestroy() => PlayerPosition.PlayerMoved -= Tick;

        void Tick(Vector3 vector3)
        {
#if UNITY_EDITOR
            if (!runInEditor)
                return;

#endif
            if (transform.localScale.x < heightReq)
                return;

            bool removedTree = false;
            List<int> toRemove = new();

            for (int l = 0; l < paryTrees.Length; l++)
            {
                TreeInstance triTree = paryTrees[l];
                Vector3 vecTree = triTree.position;
                // Get the world coordinates of the tree position
                Vector3 vec3 = Vector3.Scale(pvecTerrainSize, vecTree) + pvecTerrainPosition;
                float fltProximity = Vector3.Distance(vector3, vec3);
                if (fltProximity < removeDist)
                    toRemove.Add(l);
            }

            if (toRemove.Count <= 0) return;
            List<TreeInstance> trees = new(paryTrees);
            foreach (int i in toRemove)
            {
                int protoIndex = trees[i].prototypeIndex;
                var treeName = Terrain.activeTerrain.terrainData.treePrototypes[protoIndex].prefab.name;
                if (treeName.Contains("Tree") && !treeName.Contains("Small"))
                {
                    if (hasDebrisPool)
                        TreeDebrisObjectPool.Instance.IfHasDebrisAddFor(treeName, vector3);
                    trees.RemoveAt(i);
                    removedTree = true;
                }
            }

            paryTrees = trees.ToArray();
            Terrain.activeTerrain.terrainData.treeInstances = paryTrees;
            if (removedTree)
                RefreshTerrainColliders();
        }

        void RefreshTerrainColliders()
        {
            if (!foundCollider) return;
            terrainCollider.enabled = false;
            terrainCollider.enabled = true;
        }

#if UNITY_EDITOR
        [SerializeField] bool runInEditor;
        TreeInstance[] orgTrees;

        [ContextMenu("Restore trees")]
        public void RestoreTrees()
        {
            Terrain.activeTerrain.terrainData.treeInstances = orgTrees;
            RefreshTerrainColliders();
        }
#endif
    }
}