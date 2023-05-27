using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace AvatarStuff
{
    public sealed class AvatarScaler : MonoBehaviour
    {
        // [SerializeField] CharacterMovement characterMovement; 
        [SerializeField] float minSize = 0.1f, maxSize = 3f;
        public UnityEvent<float> SizeChange;


        bool areaHasHeightLimit;
        Coroutine heightChangeRoutine;

        float heightLimit;
        float originalHeight = 160f;
        public float Height { get; private set; }

        public void AreaHasHeightLimit(float limit)
        {
            areaHasHeightLimit = true;
            heightLimit = limit;
            if (transform.localScale.x <= limit)
                return;
            var tempHeight = Mathf.Clamp(limit, minSize, maxSize);
            if (heightChangeRoutine != null)
                StopCoroutine(heightChangeRoutine);
            heightChangeRoutine = StartCoroutine(ShrinkTo(tempHeight));
        }

        IEnumerator ShrinkTo(float endHeight)
        {
            var startSize = transform.localScale.x;
            const float duration = 0.8f;
            var startTime = Time.time;
            while (endHeight < transform.localScale.x)
            {
                var stepSize = Mathf.SmoothStep(startSize, endHeight, TimeStep());
                SetScale(stepSize);
                yield return null;
            }

            SetScale(endHeight);
            float TimeStep() => (Time.time - startTime) / duration;
        }

        void SetScale(float stepSize)
        {
            SizeChange?.Invoke(stepSize);
            transform.localScale = new Vector3(stepSize, stepSize, stepSize);
        }

        public void ExitHeightLimitArea()
        {
            areaHasHeightLimit = false;
            if (heightChangeRoutine != null)
                StopCoroutine(heightChangeRoutine);
            heightChangeRoutine = StartCoroutine(GrowTo());
            //  SetStoredHeight();
        }

        IEnumerator GrowTo()
        {
            var startSize = transform.localScale.x;
            const float duration = 1.2f;
            var startTime = Time.time;
            while (transform.localScale.x < Height)
            {
                var stepSize = Mathf.SmoothStep(startSize, Height, TimeStep());
                SetScale(stepSize);
                yield return null;
            }

            SetStoredHeight();
            float TimeStep() => (Time.time - startTime) / duration;
        }

        public void NewHeight(float avatarHeight) => originalHeight = avatarHeight;

        public void ChangeScale(float newHeight)
        {
            var limited = areaHasHeightLimit && heightLimit < newHeight;
            Height = Mathf.Clamp(newHeight / originalHeight, minSize, maxSize);
            if (limited)
                return;
            SetStoredHeight();
            //            characterMovement.groundProbingDistance = orgProbHeight * f;
        }

        void SetStoredHeight()
        {
            SetScale(Height);
        }
    }
}