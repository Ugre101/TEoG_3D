using System.Collections;
using UnityEditor;
using UnityEngine;

namespace CustomClasses
{
    public class SerializableScriptableObject : ScriptableObject
    {
        [SerializeField, HideInInspector,] string guid;

        public string Guid => guid;
#if UNITY_EDITOR
        public virtual void OnValidate()
        {
            string path = AssetDatabase.GetAssetPath(this);
            guid = AssetDatabase.AssetPathToGUID(path);
        }
#endif
    }
    public interface ISavableSerializableScriptableObject
    {
        public SerializableScriptableObjectSaves Save();
        public IEnumerator Load(SerializableScriptableObjectSaves toLoad);

    }
}