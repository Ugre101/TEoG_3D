using System;
using System.Collections.Generic;
using Safe_To_Share.Scripts.Holders;
using UnityEngine;
using UnityEngine.Events;

namespace Safe_To_Share.Scripts
{
    public sealed class InvokeEventWhenPlayerIsClose : MonoBehaviour
    {
        [SerializeField] List<InRangeOf> inRangeOfs = new();

        [Header("Settings"), SerializeField,] float frameLimit = 10f;

        [SerializeField] float exitPadding = 1f;


        void Start()
        {
            if (inRangeOfs.Count == 0)
                enabled = false;
        }

        void Update()
        {
            if (Time.frameCount % frameLimit != 0) return;
            foreach (var rangeOf in inRangeOfs)
            {
                var dist = Vector3.Distance(PlayerHolder.Position, transform.position);
                if (rangeOf.triggered && rangeOf.range + exitPadding < dist)
                {
                    rangeOf.triggered = false;
                    rangeOf.outOfRange?.Invoke();
                }
                else if (dist <= rangeOf.range)
                {
                    rangeOf.triggered = true;
                    rangeOf.inRange?.Invoke();
                }
            }
        }

        [Serializable]
        class InRangeOf
        {
            public UnityEvent inRange;
            public UnityEvent outOfRange;
            public float range;
            public bool triggered;
        }
    }
}