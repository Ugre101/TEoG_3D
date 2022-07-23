using System;
using System.Collections.Generic;
using AvatarStuff.Holders;
using UnityEngine;
using UnityEngine.Events;

namespace Safe_To_Share.Scripts
{
    public class InvokeEventWhenPlayerIsClose : MonoBehaviour
    {
        [SerializeField] List<InRangeOf> inRangeOfs = new();

        [Header("Settings"), SerializeField,]
        
        float frameLimit = 10f;

        [SerializeField] float exitPadding = 1f;

        Transform player;

        void Start()
        {
            if (PlayerHolder.Instance != null)
            {
                player = PlayerHolder.Instance.transform;
                if (inRangeOfs.Count == 0)
                    enabled = false;
            }
            else
            {
                Debug.LogWarning("Couldn't find player holder");
                enabled = false;
            }
        }

        void Update()
        {
            if (Time.frameCount % frameLimit != 0) return;
            foreach (var rangeOf in inRangeOfs)
            {
                float dist = Vector3.Distance(player.position, transform.position);
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