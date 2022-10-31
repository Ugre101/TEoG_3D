using UnityEngine;

namespace Safe_To_Share.Scripts.AvatarStuff.ScatAndPiss
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