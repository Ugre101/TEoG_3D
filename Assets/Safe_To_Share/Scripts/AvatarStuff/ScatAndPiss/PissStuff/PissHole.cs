using UnityEngine;
using UnityEngine.Pool;

namespace Safe_To_Share.Scripts.AvatarStuff.ScatAndPiss
{
    public sealed class PissHole : MonoBehaviour
    {
        [SerializeField] PissPrefab prefab;
        const float Force = 90f;
        const float DelayBetweenDrops = 0.075f;
        const float StopTime = 2f;
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
            if (timeSinceLastDrop < DelayBetweenDrops)
            {
                timeSinceLastDrop += Time.deltaTime;
                return;
            }
            Piss(transform.up * Force);
            timeSinceLastDrop = 0;
        }

        void StopRoutine()
        {
            stoppingTimeSpent += Time.deltaTime;
            timeSinceLastDrop += Time.deltaTime;
            float percent = (stoppingTimeSpent / StopTime);
            // print($"{delayBetweenDrops} + {delayBetweenDrops * percent} < {timeSinceLastDrop}");
            if (DelayBetweenDrops + (DelayBetweenDrops * percent) < timeSinceLastDrop)
            {
                
                float f = Force - Force * percent;
                
                Piss(transform.up * f);
                timeSinceLastDrop = 0;
            }

            if (StopTime < stoppingTimeSpent)
            {
                stopRoutineActive = false;
                stoppingTimeSpent = 0;
            }
        }

        float avatarHeight = 1f;
        public void StartPissing(float height)
        {
            avatarHeight = height;
            pissing = true;
        }

        public void StopPissing()
        {
            stopRoutineActive = true;
            pissing = false;
        }
     
         void Piss(Vector3 f)
        {
           var drop =  pissPool.Get();
           drop.Launch(f, pissPool);
        }

        void ActionOnRelease(PissPrefab obj)
        {
            obj.gameObject.SetActive(false);
            obj.ResetPosAndRot(transform.position,transform.forward,avatarHeight);
        }

        void ActionOnGet(PissPrefab obj) => obj.gameObject.SetActive(true);

        PissPrefab CreateFunc()
        {
            var pissPrefab = Instantiate(prefab);
            pissPrefab.ResetPosAndRot(transform.position,transform.forward,avatarHeight);
            return pissPrefab;
        }
    }
}