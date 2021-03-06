using UnityEngine;

namespace Map
{
    public abstract class MiniMapBaseObject : MonoBehaviour
    {
        [SerializeField] protected Sprite icon;
        [SerializeField] protected bool addIconToBigMap;
        [SerializeField] protected string addedIconText;
        public virtual Sprite Icon => icon;

        public Vector3 Pos => transform.position;

        public bool AddIconToBigMap => addIconToBigMap;

        public string AddedIconText => addedIconText;
    }
}