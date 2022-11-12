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

        IEnumerator LosePresure()
        {
            yield return new WaitForSeconds(pissTime);
            pissHole.StopPissing();
        }


        void TakeaShit()
        {
            scatHandler.Scat(shitSize);
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

        [ContextMenu("Test piss")]
        public void TestPiss()
        {
            pissHole.StartPissing();
            StartCoroutine(LosePresure());
        }
        
        [ContextMenu("Test Scat")]
        public void TestScat()
        {
            if (animator == null) return;
            animator.SetTrigger(shitHash);
        }
#endif

    }
}