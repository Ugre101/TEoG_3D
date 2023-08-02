using UnityEngine;

namespace Safe_To_Share.Scripts.GameEvents {
    [CreateAssetMenu(fileName = "Image Event", menuName = "Events/Image event", order = 0)]
    public sealed class ImageEvent : GameBaseEvent {
        [SerializeField] Sprite image;
        public Sprite Image => image;
    }
}