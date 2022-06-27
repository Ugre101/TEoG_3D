using System.Collections.Generic;
using AvatarStuff.Holders;
using Map;
using UnityEngine;

namespace Safe_To_Share.Scripts.Interactable
{
    public class DestroyCloseTrees : MonoBehaviour
    {
        private TreeInstance[] paryTrees;
        private Vector3 pvecTerrainPosition;
        private Vector3 pvecTerrainSize;
        [SerializeField,Range(float.Epsilon, 5f),] float removeDist = 2f;
        [SerializeField, Range(2f, 5f)] float heightReq = 3f;
        bool foundCollider;
        bool hasDebrisPool;
        TerrainCollider terrainCollider;
        void Start()
        {
            if (Terrain.activeTerrain == null)
            {
                enabled = false;
                return;
            }
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

        const int FrameLimit = 8;

        void Update()
        {
#if UNITY_EDITOR
            if (!runInEditor)
                return;
            
#endif
            if (Time.frameCount % FrameLimit != 0)
                return;
            if (transform.localScale.x < heightReq)
                return;

            bool removedTree = false;
            Vector3 vecFlier = PlayerHolder.Instance.transform.position; 
            List<int> toRemove = new();

            for (int l = 0; l < paryTrees.Length; l++)
            {
                TreeInstance triTree = paryTrees[l];
                Vector3 vecTree = triTree.position;
                // Get the world coordinates of the tree position
                Vector3 vec3 = Vector3.Scale(pvecTerrainSize, vecTree) + pvecTerrainPosition;
                float fltProximity = Vector3.Distance(vecFlier, vec3);
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
                    var treePos = PlayerHolder.Instance.transform.position;
                    if (hasDebrisPool)
                        TreeDebrisObjectPool.Instance.IfHasDebrisAddFor(treeName, treePos);
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
        [SerializeField] bool runInEditor = false;
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
 
