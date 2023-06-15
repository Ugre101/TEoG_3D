using System;

namespace CustomClasses {
    [Serializable]
    public struct DropSerializableObject<T> where T : SerializableScriptableObject {
        public string guid;
    }
}