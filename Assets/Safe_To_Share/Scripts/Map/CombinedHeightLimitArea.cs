using System.Collections.Generic;
using Safe_To_Share.Scripts.Holders;
using UnityEngine;

namespace Safe_To_Share.Scripts.Map
{
    [RequireComponent(typeof(BoxCollider))]
    public sealed class CombinedHeightLimitArea : MonoBehaviour
    {
        static readonly Dictionary<int, HashSet<CombinedHeightLimitArea>> InsideArea = new();
        [SerializeField] int sharedId;
        [SerializeField] float heightLimit = 1f;

        void OnDestroy() => InsideArea.Remove(sharedId);

        void OnTriggerEnter(Collider other)
        {
            if (!other.TryGetComponent(out Holder holder))
                return;
            if (InsideArea.TryGetValue(sharedId, out var list))
                list.Add(this);
            else if (InsideArea.TryAdd(sharedId,new HashSet<CombinedHeightLimitArea> {this}))
                holder.Scaler.AreaHasHeightLimit(heightLimit);
        }

        void OnTriggerExit(Collider other)
        {
            if (!other.TryGetComponent(out Holder holder))
                return;
            if (!InsideArea.TryGetValue(sharedId, out var hashSet)) return;
            hashSet.Remove(this);
            if (hashSet.Count > 0) return;
            holder.Scaler.ExitHeightLimitArea();
            InsideArea.Remove(sharedId);
        }
    }
}