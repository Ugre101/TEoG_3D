using System;
using System.Collections;
using UnityEngine;

namespace AvatarStuff
{
    public class ScatPrefab : MonoBehaviour
    {
        [SerializeField] Animator animator;

        [SerializeField, Range(float.Epsilon, 1f)]
        float speed= 0.5f;
        [SerializeField] float fallDist = 1f;
        [SerializeField] int playHash;
#if UNITY_EDITOR
        void OnValidate()
        {
            if (Application.isPlaying)return;
            playHash = Animator.StringToHash("Play");
        }
#endif

        public void Play()
        {
            StartCoroutine(FallOut());
        }

        IEnumerator FallOut()
        {
            float dist = 0;
            while (dist < fallDist)
            {
                var downDist = Vector3.down * speed;
                transform.localPosition += downDist;
                dist += downDist.magnitude;
                yield return null;
            }
            animator.SetTrigger(playHash);
        }
    }
}