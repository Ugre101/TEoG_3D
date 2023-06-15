using UnityEngine;

namespace Map {
    public sealed class Debris : MonoBehaviour {
        float spawnTime;

        void Update() {
            if (!TimeToDeSpawn()) return;
            gameObject.SetActive(false);
        }

        bool TimeToDeSpawn() => spawnTime + 30f < Time.time;

        public void Setup() {
            gameObject.SetActive(true);
            spawnTime = Time.time;
        }
    }
}