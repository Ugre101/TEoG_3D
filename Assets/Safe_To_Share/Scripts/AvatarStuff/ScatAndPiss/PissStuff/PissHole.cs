using System;
using UnityEngine;
using UnityEngine.Pool;

namespace Safe_To_Share.Scripts.AvatarStuff.ScatAndPiss
{
    public class PissHole : MonoBehaviour
    {
        [SerializeField] PissPrefab prefab;
        [SerializeField] float force;
        [SerializeField, Range(float.Epsilon, 0.5f)] float delayBetweenDrops = 0.5f;
        ObjectPool<PissPrefab> pissPool;
        bool pissing;
        float timeSinceLastDrop;
        void Start()
        {
            pissPool = new ObjectPool<PissPrefab>(CreateFunc,ActionOnGet,ActionOnRelease);
        }

        void Update()
        {
            if (!pissing) return;
            if (timeSinceLastDrop < delayBetweenDrops)
            {
                timeSinceLastDrop += Time.deltaTime;
                return;
            }
            Piss();
            timeSinceLastDrop = 0;
        }

        [ContextMenu("Start Pissing")]
        public void StartPissing()
        {
            pissing = true;
        }

        [ContextMenu("Stop Pissing")]
        public void StopPissing()
        {
            pissing = false;
        }
        
        [ContextMenu("One Piss")]
        public void Piss()
        {
           var drop =  pissPool.Get();
           drop.Launch(new Vector3(0,force,0), pissPool);
        }

        void ActionOnRelease(PissPrefab obj)
        {
            obj.gameObject.SetActive(false);
            obj.ResetPosAndRot();
            print("Returned");
        }

        void ActionOnGet(PissPrefab obj)
        {
            obj.gameObject.SetActive(true);
        }

        PissPrefab CreateFunc()
        {
            var pissPrefab = Instantiate(prefab, transform);
            pissPrefab.ResetPosAndRot();
            return pissPrefab;
        }
    }
}