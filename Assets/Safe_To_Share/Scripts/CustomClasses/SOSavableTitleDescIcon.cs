using CustomClasses;
using UnityEngine;

namespace Safe_to_Share.Scripts.CustomClasses {
    public class SObjSavableTitleDescIcon : SerializableScriptableObject {
        [SerializeField] string title;
        [SerializeField, TextArea,] string desc;
        [SerializeField] Sprite sprite;
        public string Title => title;
        public string Desc => desc;
        public Sprite Icon => sprite;
    }
}