using Safe_To_Share.Scripts.Static;
using UnityEngine;

namespace Safe_To_Share.Scripts.Map {
    public sealed class DisableWhenOutOfRange : MonoBehaviour {
        [SerializeField, Range(100f, 500f),] float cullingRange;

        bool active = true;
        Transform playerTrans;

        void Start() {
            var player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
                playerTrans = player.transform;
            else
                Debug.LogError("Can't find player");
        }

        void Update() {
            if (Time.frameCount % 12 != 0)
                return;
            var inRange = Vector3.Distance(transform.position, playerTrans.position) <= cullingRange;
            switch (inRange) {
                case true when !active:
                    transform.AwakeChildren();
                    active = true;
                    break;
                case false when active:
                    transform.SleepChildren();
                    active = false;
                    break;
            }
        }
    }
}