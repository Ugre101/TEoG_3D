using System.Linq;
using UnityEngine;
using UnityEngine.Pool;

namespace Safe_To_Share.Scripts.AvatarStuff.ScatAndPiss {
    public sealed class PissPrefab : MonoBehaviour {
        const float MaxFlyTime = 10f;
        [SerializeField] Rigidbody rigid;
        [SerializeField] PissPuddle puddle;
        [SerializeField] Vector3 orgSize;

        float beenActiveTime;
        IObjectPool<PissPrefab> pool;

        void Update() {
            beenActiveTime += Time.deltaTime;
            if (MaxFlyTime < beenActiveTime)
                pool.Release(this);
        }

        void OnCollisionEnter(Collision collision) {
            if (NotValidTarget(collision)) return;
            CheckPuddle(rigid.position, collision.transform.rotation);
            pool.Release(this);
        }

        public void Launch(Vector3 dir, IObjectPool<PissPrefab> spawnPool) {
            pool = spawnPool;
            rigid.AddRelativeForce(dir);
        }

        bool NotValidTarget(Collision collision) => false;

        public void ResetPosAndRot(Vector3 transformPosition, Vector3 direction, float scaleFactor) {
            rigid.velocity = Vector3.zero;
            rigid.angularVelocity = Vector3.zero;
            transform.SetLocalPositionAndRotation(transformPosition, Quaternion.Euler(direction));
            transform.localScale = orgSize * scaleFactor;
            beenActiveTime = 0;
        }


        public void CheckPuddle(Vector3 cords, Quaternion transformRotation) {
            foreach (var existingPuddle in PissPuddle.ExistingPuddles.Where(existingPuddle =>
                         Vector3.Distance(cords, existingPuddle.transform.position) < 5f)) {
                existingPuddle.Grow();
                return;
            }

            var newPuddle = Instantiate(puddle, cords, transformRotation);
            PissPuddle.ExistingPuddles.Add(newPuddle);
        }
    }
}