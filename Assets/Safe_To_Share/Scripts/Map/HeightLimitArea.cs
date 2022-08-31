using System;
using System.Collections;
using AvatarStuff.Holders;
using UnityEngine;

namespace Safe_To_Share.Scripts.Map
{
    [RequireComponent(typeof(Collider))]
    public class HeightLimitArea : MonoBehaviour
    {
        [SerializeField] float heightLimit = 1f;

        [SerializeField,Range(0.5f,2f)] float delay = 1f;
        Coroutine routine;
        WaitForSecondsRealtime waitForSecondsRealtime;


        void Start()
        {
            waitForSecondsRealtime = new WaitForSecondsRealtime(delay);
        }

        void OnTriggerEnter(Collider other)
        {
            if (!other.TryGetComponent(out Holder holder))
                return;
            holder.Scaler.AreaHasHeightLimit(heightLimit);
            if (routine == null) return;
            StopCoroutine(routine);
            routine = null;
        }

        void OnTriggerExit(Collider other)
        {
            if (!other.TryGetComponent(out Holder holder))
                return;
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