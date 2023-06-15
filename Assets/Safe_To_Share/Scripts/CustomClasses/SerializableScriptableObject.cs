using UnityEditor;
using UnityEngine;

namespace CustomClasses {
    public class SerializableScriptableObject : ScriptableObject {
        [SerializeField, HideInInspector,] string guid;

        public string Guid => guid;
#if UNITY_EDITOR
        public virtual void OnValidate() {
            var path = AssetDatabase.GetAssetPath(this);
            guid = AssetDatabase.AssetPathToGUID(path);
        }
#endif
    }
}