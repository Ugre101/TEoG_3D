using System;
using UnityEngine;

namespace AvatarStuff
{
    public class PissPrefab : MonoBehaviour
    {
        [SerializeField] Rigidbody rigid;
        public void Launch(Vector3 dir)
        {
            rigid.AddRelativeForce(dir);
        }

        void OnCollisionStay(Collision collisionInfo)
        {
            
        }
    }
}