using System.Collections;
using UnityEngine;

namespace Safe_To_Share.Scripts.AvatarStuff.ScatAndPiss
{
    public sealed class ScatPrefab : MonoBehaviour
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
            var normal = collisionInfo.GetContact(0).normal;
            var angle = Vector3.Angle(normal, Vector3.up);
            transform.SetPositionAndRotation(transform.position + Vector3.up * upMulti,Quaternion.identity);
            transform.Rotate(0, Random.Range(0, 360), angle);
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
            rigid.velocity = Vector3.zero;
            rigid.angularVelocity = Vector3.zero;
            rigid.constraints = RigidbodyConstraints.FreezeAll;
            coll.enabled = false;
        }

        bool IsAvatarHolder(Collision collisionInfo)
        {
            if (collisionInfo.gameObject.CompareTag("Player")) return true;
            return false;
        }

        IEnumerator DelayedDestroy()
        {
            yield return waitForSeconds;
            Destroy(gameObject);
        }
    }
}