using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

namespace Safe_To_Share.Scripts.AvatarStuff.ScatAndPiss
{
    public class PissHole : MonoBehaviour
    {
        [SerializeField] PissPrefab prefab;
        [SerializeField] float force;
        [SerializeField, Range(float.Epsilon, 0.3f)] float delayBetweenDrops = 0.2f;
        [SerializeField,Range(1f,4f)] float stopTime = 2f;
        ObjectPool<PissPrefab> pissPool;
        bool pissing;
        bool stopRoutineActive;
        float timeSinceLastDrop;
        float stoppingTimeSpent;
        void Start()
        {
            pissPool = new ObjectPool<PissPrefab>(CreateFunc,ActionOnGet,ActionOnRelease);
        }

        
        void Update()
        {
            if (stopRoutineActive)
            {
                StopRoutine();
                return;
            }
            if (!pissing) return;
            if (timeSinceLastDrop < delayBetweenDrops)
            {
                timeSinceLastDrop += Time.deltaTime;
                return;
            }
            Piss(force);
            timeSinceLastDrop = 0;
        }

        void StopRoutine()
        {
            stoppingTimeSpent += Time.deltaTime;
            timeSinceLastDrop += Time.deltaTime;
            float percent = (stoppingTimeSpent / stopTime);
            // print($"{delayBetweenDrops} + {delayBetweenDrops * percent} < {timeSinceLastDrop}");
            if (delayBetweenDrops + (delayBetweenDrops * percent) < timeSinceLastDrop)
            {
                float f = force - force * percent;
                Piss(f);
                timeSinceLastDrop = 0;
            }

            if (stopTime < stoppingTimeSpent)
            {
                stopRoutineActive = false;
                stoppingTimeSpent = 0;
            }
        }

        [ContextMenu("Start Pissing")]
        public void StartPissing() => pissing = true;

        [ContextMenu("Stop Pissing")]
        public void StopPissing()
        {
            stopRoutineActive = true;
            pissing = false;
        }

     

        [ContextMenu("One Piss")]
        public void Piss(float f)
        {
           var drop =  pissPool.Get();
           drop.Launch(new Vector3(0,f,0), pissPool);
        }

        void ActionOnRelease(PissPrefab obj)
        {
            obj.gameObject.SetActive(false);
            obj.ResetPosAndRot();
        }

        void ActionOnGet(PissPrefab obj) => obj.gameObject.SetActive(true);

        PissPrefab CreateFunc()
        {
            var pissPrefab = Instantiate(prefab, transform);
            pissPrefab.ResetPosAndRot();
            return pissPrefab;
        }
    }
}