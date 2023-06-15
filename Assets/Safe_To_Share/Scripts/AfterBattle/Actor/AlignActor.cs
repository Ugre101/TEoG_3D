using UnityEngine;

namespace Safe_To_Share.Scripts.AfterBattle.Actor {
    public sealed class AlignActor : MonoBehaviour {
        static bool bothStopped;
        [SerializeField, HideInInspector,] Transform trans; // not sure if worth it but I did it to shut up rider.

        [SerializeField, Range(2f, 5f),]
        float maxDelta = 0.1f;

        [SerializeField, Range(float.Epsilon, 0.1f),]
        float yTolerance = 0.5f;

        [SerializeField, Range(float.Epsilon, 0.25f),]
        float xTolerance = 0.5f;

        bool grounded;
        Vector3 moveXDeltaBy = Vector3.zero;

        Vector3 moveYDeltaBy = Vector3.zero;
        bool moving;
        float timeSinceLastMoved;
        Transform toAlign, target;
        bool xStartedPositive;
        void Update() {
            if (bothStopped && moving)
                Stop();
            if (!moving) return;
            var delta = maxDelta * Time.deltaTime;
            var diff = toAlign.position - target.position;
            var moved = MoveAlongYAxis(delta, diff);

            MoveXAlongXAxis(delta, diff, moved);
        }
#if UNITY_EDITOR
        void OnValidate() {
            if (trans == null)
                trans = transform;
        }
#endif

        Vector3 MoveYBy(float delta) {
            moveYDeltaBy.y = delta;
            return moveYDeltaBy;
        }

        Vector3 MoveXBy(float delta) {
            moveXDeltaBy.x = delta;
            return moveXDeltaBy;
        }

        bool MoveAlongYAxis(float delta, Vector3 diff) {
            var yDelta = Mathf.Min(delta * Mathf.Abs(diff.y), Mathf.Abs(diff.y));
            if (grounded)
                yDelta = 0;
            if (diff.y < -yTolerance)
                trans.position += MoveYBy(yDelta);
            else if (diff.y > yTolerance)
                trans.position -= MoveYBy(yDelta);
            else
                return false;
            return true;
        }

        void MoveXAlongXAxis(float delta, Vector3 diff, bool moved) {
            var xDelta = Mathf.Min(delta / 2f * Mathf.Abs(diff.x), Mathf.Abs(diff.x));
            if (xStartedPositive) {
                if (diff.x < 0 + xTolerance / 2)
                    trans.position += MoveXBy(xDelta);
                else if (diff.x > xTolerance)
                    trans.position -= MoveXBy(xDelta);
                else if (moved)
                    TickTimeSpent();
            } else {
                if (diff.x < -xTolerance)
                    trans.position += MoveXBy(xDelta);
                else if (diff.x > 0 - xTolerance / 2)
                    trans.position -= MoveXBy(xDelta);
                else if (moved)
                    TickTimeSpent();
            }
        }


        void TickTimeSpent() {
            timeSinceLastMoved += Time.deltaTime;
            if (timeSinceLastMoved > 0.5f)
                moving = false;
        }

        public void Stop() {
            moving = false;
            toAlign = null;
            target = null;
        }

        public void AlignWith(Transform mine, Transform theirs, bool stayGrounded) {
            if (theirs == null || mine == null) return;
            moving = true;
            toAlign = mine;
            target = theirs;
            timeSinceLastMoved = 0f;
            var diff = toAlign.position - target.position;
            xStartedPositive = diff.x > 0;
            grounded = stayGrounded;
        }

        public static void NewAvatar(bool pause) => bothStopped = pause;
    }
}