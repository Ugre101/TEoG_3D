using Character.Organs.OrgansContainers;
using UnityEngine;

namespace AvatarStuff
{
    public sealed class DazBallsController : MonoBehaviour
    {
        [SerializeField, Min(0f),] float minSize, maxSize = 4f;
        [SerializeField] Vector3 hideOffset = new(0, -0.1f, -0.05f);
        [SerializeField, Range(0.1f, 2f),] float sizeMulti = 1f;
        public float currentSize;
        float fluidFactor = 0.8f;
        float fluidMax = 100;
        bool hidden;

        void HideBalls()
        {
            if (hidden)
                return;
            transform.localPosition += hideOffset;
            transform.localScale = new Vector3(1, 1, 1);
            hidden = true;
        }

        public void ShowOrHide(bool show)
        {
            if (show)
                ShowBalls();
            else
                HideBalls();
        }

        void ShowBalls()
        {
            if (!hidden)
                return;
            transform.localPosition -= hideOffset;
            hidden = false;
        }

        public void ReSize(float newSize)
        {
            float size = Mathf.Clamp(newSize, minSize, maxSize);
            currentSize = size;
            SetBallSize();
        }

        void SetBallSize()
        {
            float finalSize = currentSize * fluidFactor * sizeMulti;
            transform.localScale = new Vector3(finalSize, finalSize, finalSize);
        }

        public void SetupFluidStretch(BaseOrgansContainer container)
        {
            fluidMax = container.Fluid.Value;
            SetFluidStretch(container.Fluid.CurrentValue);
        }

        public void SetFluidStretch(float current) => fluidFactor = Mathf.Clamp(current / fluidMax, 0.8f, 1.1f);
    }
}