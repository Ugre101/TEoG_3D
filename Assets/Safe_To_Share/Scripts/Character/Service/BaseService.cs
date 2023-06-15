using Character.PlayerStuff;
using UnityEngine;

namespace Character.Service {
    public abstract class BaseService : ScriptableObject {
        [SerializeField] string title;
        [SerializeField, TextArea,] string desc;
        [SerializeField] int cost;

        public string Title => title;

        public string Desc => desc;

        public int Cost => cost;

        public virtual void OnUse(Player player) { }
    }
}