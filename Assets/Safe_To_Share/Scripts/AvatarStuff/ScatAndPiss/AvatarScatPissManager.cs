using System.Collections;
using UnityEngine;

namespace Safe_To_Share.Scripts.AvatarStuff.ScatAndPiss
{
    public sealed class AvatarScatPissManager : MonoBehaviour
    {
        [SerializeField] Animator animator;
        [SerializeField] int shitHash;
        [SerializeField] ScatHandler scatHandler;
        [Header("Pissing")] [SerializeField] PissHole pissHole;
        [SerializeField] [Range(1f, 10f)] float pissTime = 5f;

        IEnumerator LosePressure(float bladderCurrent)
        {
            yield return new WaitForSeconds(bladderCurrent);
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
                else 
                    print($"{gameObject.name} has no scatHandler");
            }

            if (pissHole == null)
            {
                var found = GetComponentInChildren<PissHole>();
                if (found != null)
                    pissHole = found;
                else
                    print($"{gameObject.name} has no pissHole");
            }
        }
#endif

        [ContextMenu("Test piss")]
        void TestPiss() => Piss(pissTime,1f);

        public void Piss(float current, float height)
        {
            pissHole.StartPissing(height);
            StartCoroutine(LosePressure(current * 1.2f));
        }

        float avatarHeight;
        [ContextMenu("Test Scat")]
        void TextScat() => Scat(1f);
        public void Scat(float height)
        {
            if (animator == null) return;
            avatarHeight = height;
            animator.SetTrigger(shitHash);
        }

        void TakeaShit()
        {
            scatHandler.Scat(avatarHeight);
        }

    }
}