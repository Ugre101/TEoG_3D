using UnityEngine;

namespace Safe_To_Share.Scripts.Movement.HoverMovement {
    public static class MovementTools {
        public static Vector3 FlatVel(this Vector3 vel) {
            vel.y = 0;
            return vel;
        }
    }
}