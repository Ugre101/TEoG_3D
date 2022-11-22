using System.Collections;
using UnityEngine;

namespace Safe_To_Share.Scripts.AvatarStuff.ScatAndPiss
{
    public class ScatAvatarTester : MonoBehaviour
    {
        [SerializeField] Animator animator;
        [SerializeField] int shitHash;
        [SerializeField] ScatHandler scatHandler;
        [SerializeField] [Range(0.5f, 5f)] float shitSize = 1f;
        [Header("Pissing")] [SerializeField] PissHole pissHole;
        [SerializeField] [Range(1f, 10f)] float pissTime = 1f;

        IEnumerator LosePressure()
        {
            yield return new WaitForSeconds(pissTime);
            pissHole.StopPissing();
        }


#if UNITY_EDITOR
        void OnValidate()
        {
            if (animator == null && TryGetComponent(out Animator ani))
                animator = ani;
            shitHash = Animator.StringToHash("Shit");
            if (scatHandler == null)
            {
                var found = GetComponentInChildren<ScatHandler>();
                if (found != null)
                    scatHandler = found;
            }

            if (pissHole == null)
            {
                var found = GetComponentInChildren<PissHole>();
                if (found != null)
                    pissHole = found;
            }
        }
#endif

        [ContextMenu("Test piss")]
        public void Piss()
        {
            pissHole.StartPissing();
            StartCoroutine(LosePressure());
        }
        
        [ContextMenu("Test Scat")]
        public void Scat()
        {
            if (animator == null) return;
            animator.SetTrigger(shitHash);
        }

    }
}