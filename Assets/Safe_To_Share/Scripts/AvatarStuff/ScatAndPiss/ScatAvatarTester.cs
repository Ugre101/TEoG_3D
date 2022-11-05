using System.Collections;
using UnityEngine;

namespace Safe_To_Share.Scripts.AvatarStuff.ScatAndPiss
{
    public class ScatAvatarTester : MonoBehaviour
    {
        [SerializeField] Animator animator;
        [SerializeField] int shitHash;
        [SerializeField] ScatHandler scatHandler;
        [SerializeField, Range(0.5f, 5f)] float shitSize = 1f;
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
        }
        
        [ContextMenu("Test Scat")]
        public void Test()
        {
            if (animator == null) return;
            animator.SetTrigger(shitHash);
        }

        void TakeaShit()
        {
            scatHandler.Scat(shitSize);
        }
    }
}