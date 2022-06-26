using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Map
{
    public class TreeDebrisObjectPool : MonoBehaviour
    {
        public static TreeDebrisObjectPool Instance { get; private set; }
        [SerializeField] List<DebrisMatch> debrisMatches = new();

        Dictionary<string, DebrisMatch> dict;

        Dictionary<string, DebrisMatch> Dict => dict ??= debrisMatches.ToDictionary(dm => dm.TreeName);

        void Awake()
        {
            if (Instance == null)
                Instance = this;
            else   
                Destroy(gameObject);
        }

        public void IfHasDebrisAddFor(string prefabName, Vector3 treePos)
        {
            if (!Dict.TryGetValue(prefabName, out var match)) return;
            Debris prefab = match.GetPrefab();
            prefab.Setup();
            prefab.transform.position = treePos;
        }

#if UNITY_EDITOR

        [SerializeField] List<EditorDebrisMatch> editorDebrisMatches = new();
        [ContextMenu("Setup matches")]
        void SetupMatches()
        {
            for (int i = transform.childCount; i --> 0;) 
                DestroyImmediate(transform.GetChild(i).gameObject);
            debrisMatches = new List<DebrisMatch>();
            foreach (EditorDebrisMatch match in editorDebrisMatches)
                if (match.debrisPrefab != null && match.treePrefab != null)
                {
                    List<Debris> prefabs = new();
                    for (int i = 0; i < 10; i++) 
                        prefabs.Add(AddDebris(match));
                    debrisMatches.Add(new DebrisMatch(match.treePrefab.name, prefabs));
                }
        }

        Debris AddDebris(EditorDebrisMatch match)
        {
            var instantiate = Instantiate(match.debrisPrefab, transform).AddComponent<Debris>();
            if (instantiate.TryGetComponent(out Collider col))
                col.enabled = false;
            instantiate.gameObject.SetActive(false);
            return instantiate;
        }

        [Serializable]
        struct EditorDebrisMatch
        {
            public GameObject treePrefab;
            public GameObject debrisPrefab;
        }
#endif
        
        [Serializable]
        class DebrisMatch
        {
            public DebrisMatch(string treeName, List<Debris> prefab)
            {
                this.treeName = treeName;
                prefabs = prefab;
            }

            int id;
            [SerializeField] string treeName;
            [SerializeField] List<Debris> prefabs;

            public Debris GetPrefab()
            {
                if (id >= 0)
                    id = 0;
                return prefabs[id++];
            }

            public string TreeName => treeName;
        }
        
    }
}