using System.Collections.Generic;
using AvatarStuff.Holders;
using UnityEngine;

namespace Safe_To_Share.Scripts.Map
{
    [RequireComponent(typeof(BoxCollider))]
    public class CombinedHeightLimitArea : MonoBehaviour
    {
        static readonly HashSet<CombinedHeightLimitArea> InsideArea = new();
        [SerializeField] int sharedId;
        [SerializeField] float heightLimit = 1f;

        void OnDestroy() => InsideArea.Remove(this);

        void OnTriggerEnter(Collider other)
        {
            if (!other.TryGetComponent(out Holder holder))
                return;
            holder.Scaler.AreaHasHeightLimit(heightLimit);
            InsideArea.Add(this);
        }

        void OnTriggerExit(Collider other)
        {
            if (!other.TryGetComponent(out Holder holder))
                return;
            InsideArea.Remove(this);
            if (InsideArea.Count > 0)
                return;
            holder.Scaler.ExitHeightLimitArea();
        }
    }
}