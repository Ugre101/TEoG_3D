using System;
using System.Collections;
using AvatarScripts;
using UnityEngine;
using UnityEngine.Events;

namespace AvatarStuff
{
    public class AvatarScaler : MonoBehaviour
    {
       [SerializeField] CharacterCapsule characterCapsule;
      // [SerializeField] CharacterMovement characterMovement; 
       [SerializeField] float minSize = 0.1f, maxSize = 3f;


        bool areaHasHeightLimit;
        bool firstUse = true;
        public float Height { get; private set; }
        public UnityEvent<float> SizeChange;
        Coroutine heightChangeRoutine;
       [SerializeField,HideInInspector] float orgCapWidth, orgCapHeight;
        float originalHeight = 160f;

        public void AreaHasHeightLimit(float limit)
        {
            areaHasHeightLimit = true;
            if (transform.localScale.x <= limit)
                return;
            float tempHeight = Mathf.Clamp(limit, minSize, maxSize);
            if (heightChangeRoutine != null)
                StopCoroutine(heightChangeRoutine);
            heightChangeRoutine = StartCoroutine(ShrinkTo(tempHeight));
        }

        IEnumerator ShrinkTo(float endHeight)
        {
            float startSize = transform.localScale.x;
            const float duration = 0.8f;
            float startTime = Time.time;
            while (endHeight < transform.localScale.x)
            {
                float stepSize = Mathf.SmoothStep(startSize, endHeight, TimeStep());
                SetScale(stepSize);
                yield return null;
            }

            characterCapsule.SetHeight( orgCapHeight * endHeight);
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
            float startSize = transform.localScale.x;
            const float duration = 1.2f;
            float startTime = Time.time;
            while (transform.localScale.x < Height)
            {
                float stepSize = Mathf.SmoothStep(startSize, Height, TimeStep());
                SetScale(stepSize);
                yield return null;
            }

            SetStoredHeight();
            float TimeStep() => (Time.time - startTime) / duration;
        }

        public void NewHeight(float avatarHeight) => originalHeight = avatarHeight;

        public void ChangeScale(float newHeight)
        {
            Height = Mathf.Clamp(newHeight / originalHeight, minSize, maxSize);
            if (areaHasHeightLimit)
                return;
            SetStoredHeight();
            //            characterMovement.groundProbingDistance = orgProbHeight * f;
        }

        void SetStoredHeight()
        {
            characterCapsule.SetHeight(orgCapHeight * Height);
            SetScale(Height);
        }

        void OnValidate()
        {
            if (Application.isPlaying)
                return;
            if (characterCapsule == null)
            {
                throw new MissingComponentException("Char Capsule is missing on " + transform.parent.name);
            }
            orgCapWidth = characterCapsule.Radius;
            orgCapHeight = characterCapsule.Height;
        }
    }
}