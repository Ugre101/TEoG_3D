using System.Collections;
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
        [SerializeField, Range(0.5f, 2f),] float delay = 1f;
        Coroutine routine;
        WaitForSecondsRealtime waitForSecondsRealtime;


        void Start()
        {
            waitForSecondsRealtime = new WaitForSecondsRealtime(delay);
        }

        void OnDestroy()
        {
            InsideArea.Remove(sharedId);
            StopAllCoroutines();
        }

        void OnTriggerEnter(Collider other)
        {
            if (!other.TryGetComponent(out Holder holder))
                return;
            if (InsideArea.TryGetValue(sharedId, out var list))
                list.Add(this);
            else if (InsideArea.TryAdd(sharedId, new HashSet<CombinedHeightLimitArea> { this, }))
                holder.Scaler.AreaHasHeightLimit(heightLimit);
            if (routine == null) return;
            StopCoroutine(routine);
            routine = null;
        }

        void OnTriggerExit(Collider other)
        {
            if (!other.TryGetComponent(out Holder holder))
                return;
            if (!InsideArea.TryGetValue(sharedId, out var hashSet)) return;
            hashSet.Remove(this);
            if (hashSet.Count > 0) return;
            // holder.Scaler.ExitHeightLimitArea();
            InsideArea.Remove(sharedId);
            routine ??= StartCoroutine(DelayExit(holder));
        }

        IEnumerator DelayExit(Holder holder)
        {
            yield return waitForSecondsRealtime;
            holder.Scaler.ExitHeightLimitArea();
            routine = null;
        }
    }
}