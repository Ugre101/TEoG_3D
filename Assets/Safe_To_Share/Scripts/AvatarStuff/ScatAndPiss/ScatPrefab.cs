using System.Collections;
using UnityEngine;

namespace Safe_To_Share.Scripts.AvatarStuff.ScatAndPiss
{
    public class ScatPrefab : MonoBehaviour
    {
        [SerializeField] Animation animator;
        [SerializeField] Rigidbody rigid;
        [SerializeField] Collider coll;

        [SerializeField] [Range(float.Epsilon, 0.05f)]
        float upMulti = 0.03f;

        [SerializeField] new Transform transform;
        readonly WaitForSeconds waitForSeconds = new(10f);

        void OnCollisionEnter(Collision collisionInfo)
        {
            if (IsAvatarHolder(collisionInfo)) return;
            Freeze();
            transform.SetPositionAndRotation(transform.position + transform.up * upMulti,
                collisionInfo.transform.rotation);
            transform.Rotate(0, Random.Range(0, 360), 0);
            foreach (AnimationState state in animator) state.speed = VelocityGivenSpeed();
            animator.Play();
            StartCoroutine(DelayedDestroy());
        }

        float VelocityGivenSpeed()
        {
            // TODO  float speed = rigid.velocity.magnitude;
            return 2f;
        }

        void Freeze()
        {
            rigid.constraints = RigidbodyConstraints.FreezeAll;
            coll.enabled = false;
        }

        bool IsAvatarHolder(Collision collisionInfo)
        {
            return false;
        }

        IEnumerator DelayedDestroy()
        {
            yield return waitForSeconds;
            Destroy(gameObject);
        }
    }
}