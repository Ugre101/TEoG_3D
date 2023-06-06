using UnityEngine;

namespace Safe_To_Share.Scripts.Movement.HoverMovement
{
    public sealed class ActorOrientation : MonoBehaviour
    {
        [SerializeField] Rigidbody rb;
        [SerializeField] Transform cameraTransform;
        [SerializeField] Transform actorTransform;

        void Awake()
        {
            if (actorTransform == null)
                enabled = false;
        }

        void Update()
        {
            if (rb.velocity.FlatVel().magnitude <= 0.2f) 
                return;
            UpdateRotation();
        }

#if UNITY_EDITOR
        void OnValidate()
        {
            if (cameraTransform == null && Camera.main != null)
                cameraTransform = Camera.main.transform;
        }
#endif


        public void SetActorTransform(Transform trans)
        {
            actorTransform = trans;
            enabled = true;
        }

        public void SetActorWithAnimator(Animator ani) => SetActorTransform(ani.transform);

        void UpdateRotation() => actorTransform.rotation = Quaternion.Euler(0, cameraTransform.eulerAngles.y, 0);
    }
}