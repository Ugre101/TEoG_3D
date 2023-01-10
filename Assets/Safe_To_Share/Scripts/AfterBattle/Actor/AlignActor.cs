using UnityEngine;

namespace Safe_To_Share.Scripts.AfterBattle.Actor
{
    public class AlignActor : MonoBehaviour
    {
        [SerializeField, HideInInspector,] Transform trans; // not sure if worth it but I did it to shut up rider.

        [SerializeField, Range(2f, 5f),]
        float maxDelta = 0.1f;

        [SerializeField, Range(float.Epsilon, 0.1f),]
        float yTolerance = 0.5f;

        [SerializeField, Range(float.Epsilon, 0.25f),]
        float xTolerance = 0.5f;
        bool moving;
        bool xStartedPositive;
        Transform toAlign, target;
        float timeSinceLastMoved;
        void Update()
        {
            if (!moving) return;
            var delta = maxDelta * Time.deltaTime;
            bool moved = true;
            var diff = toAlign.position - target.position;
            float yDelta = Mathf.Min(delta * Mathf.Abs(diff.y), Mathf.Abs(diff.y));
            if (diff.y < -yTolerance)
                trans.position += new Vector3(0, yDelta, 0);
            else if (diff.y > yTolerance)
                trans.position -= new Vector3(0, yDelta, 0);
            else
                moved = false;

            float xDelta = Mathf.Min(delta / 2f * Mathf.Abs(diff.x), Mathf.Abs(diff.x));
            if (xStartedPositive)
            {
                if (diff.x < 0 + xTolerance / 2)
                    trans.position += new Vector3(xDelta, 0, 0);
                else if (diff.x > xTolerance)
                    trans.position -= new Vector3(xDelta, 0, 0);
                else if (moved) 
                    TickTimeSpent();
            }
            else
            {
                if (diff.x < -xTolerance )
                    trans.position += new Vector3(xDelta, 0, 0);
                else if (diff.x > 0 - xTolerance / 2)
                    trans.position -= new Vector3(xDelta, 0, 0);
                else if (moved) 
                    TickTimeSpent();
            }
        }

        void TickTimeSpent()
        {
            timeSinceLastMoved += Time.deltaTime;
            if (timeSinceLastMoved > 0.5f)
                moving = false;
        }
#if UNITY_EDITOR
        void OnValidate()
        {
            if (trans == null)
                trans = transform;
        }
#endif

        public void Stop()
        {
            moving = false;
            toAlign = null;
            target = null;
        }
        public void AlignWith(Transform mine, Transform theirs)
        {
            if (theirs == null || mine == null) return;
            moving = true;
            toAlign = mine;
            target = theirs;
            timeSinceLastMoved = 0f;
            var diff = toAlign.position - target.position;
            xStartedPositive = diff.x > 0;
        }
    }
}