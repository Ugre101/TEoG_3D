using UnityEngine;

namespace Safe_To_Share.Scripts
{
    public class NpcAnimationController : MonoBehaviour
    {
        [SerializeField] bool sitting;
        [SerializeField] Animator animator;
        static readonly int Sitting = Animator.StringToHash("Sitting");

        void Start() => animator.SetBool(Sitting, sitting);
    }
}
