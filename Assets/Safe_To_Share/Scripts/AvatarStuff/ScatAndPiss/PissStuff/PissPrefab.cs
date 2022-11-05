using System.Linq;
using UnityEngine;
using UnityEngine.Pool;

namespace Safe_To_Share.Scripts.AvatarStuff.ScatAndPiss
{
    public class PissPrefab : MonoBehaviour
    {
        const float MaxFlyTime = 10f;
        [SerializeField] Rigidbody rigid;
        [SerializeField] PissPuddle puddle;
        float beenActiveTime;
        IObjectPool<PissPrefab> pool;

        void Update()
        {
            beenActiveTime += Time.deltaTime;
            if (MaxFlyTime < beenActiveTime)
                pool.Release(this);
        }

        void OnCollisionEnter(Collision collision)
        {
            if (NotValidTarget(collision)) return;
            CheckPuddle(rigid.position, collision.transform.rotation);
            pool.Release(this);
        }

        public void Launch(Vector3 dir, IObjectPool<PissPrefab> spawnPool)
        {
            pool = spawnPool;
            rigid.AddRelativeForce(dir);
        }

        bool NotValidTarget(Collision collision)
        {
            return false;
        }

        public void ResetPosAndRot()
        {
            rigid.velocity = Vector3.zero;
            rigid.angularVelocity = Vector3.zero;
            transform.SetLocalPositionAndRotation(Vector3.zero, new Quaternion());
            beenActiveTime = 0;
        }


        public void CheckPuddle(Vector3 cords, Quaternion transformRotation)
        {
            foreach (var existingPuddle in PissPuddle.ExistingPuddles.Where(existingPuddle => Vector3.Distance(cords, existingPuddle.transform.position) < 5f))
            {
                existingPuddle.Grow();
                return;
            }
            var newPuddle = Instantiate(puddle, cords + new Vector3(0,0.1f,0), transformRotation);
            PissPuddle.ExistingPuddles.Add(newPuddle);
        }
    }
}