using Safe_To_Share.Scripts.Holders.AI.StateMachineStuff;
using UnityEngine;

namespace AvatarStuff.Holders.AI.StateMachineStuff.DormMateBrain {
    public class DormMateIdle : State<DormMateAiHolder> {
        float waitUntil;

        public DormMateIdle(DormMateAiHolder behaviour) : base(behaviour) { }

        public override void OnEnter() {
            waitUntil = Time.time + Random.Range(3f, 10f);
            Behaviour.Agent.ResetPath();
        }

        public override void OnUpdate() {
            if (Time.time >= waitUntil) { }
        }
    }
}