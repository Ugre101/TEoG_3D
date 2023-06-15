using UnityEngine;

namespace Safe_To_Share.Scripts {
    public sealed class NpcAnimationController : MonoBehaviour {
        static readonly int Sitting = Animator.StringToHash("Sitting");
        [SerializeField] bool sitting;
        [SerializeField] Animator animator;

        void Start() => animator.SetBool(Sitting, sitting);
    }
}