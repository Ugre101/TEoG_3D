using UnityEngine;

namespace Safe_To_Share.Scripts.Holders.AI {
    [RequireComponent(typeof(Animator))]
    public sealed class LookAt : MonoBehaviour {
        public Transform head;
        public Vector3 lookAtTargetPosition;
        public float lookAtCoolTime = 0.2f;
        public float lookAtHeatTime = 0.2f;
        public bool looking = true;
        Animator animator;

        Vector3 lookAtPosition;
        float lookAtWeight;

        void Start() {
            if (!head) {
                Debug.LogError("No head transform - LookAt disabled");
                enabled = false;
                return;
            }

            animator = GetComponent<Animator>();
            lookAtTargetPosition = head.position + transform.forward;
            lookAtPosition = lookAtTargetPosition;
        }

        void OnAnimatorIK(int layerIndex) {
            var position = head.position;
            lookAtTargetPosition.y = position.y;
            var lookAtTargetWeight = looking ? 1.0f : 0.0f;

            var curDir = lookAtPosition - position;
            var futDir = lookAtTargetPosition - position;

            curDir = Vector3.RotateTowards(curDir, futDir, 6.28f * Time.deltaTime, float.PositiveInfinity);
            lookAtPosition = position + curDir;

            var blendTime = lookAtTargetWeight > lookAtWeight ? lookAtHeatTime : lookAtCoolTime;
            lookAtWeight = Mathf.MoveTowards(lookAtWeight, lookAtTargetWeight, Time.deltaTime / blendTime);
            animator.SetLookAtWeight(lookAtWeight, 0.2f, 0.5f, 0.7f, 0.5f);
            animator.SetLookAtPosition(lookAtPosition);
        }
    }
}