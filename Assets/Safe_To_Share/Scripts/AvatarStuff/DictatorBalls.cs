using Character.Organs.OrgansContainers;
using UnityEngine;

namespace AvatarStuff {
    public sealed class DictatorBalls : MonoBehaviour {
        [SerializeField, Min(0f),] float minSize, maxSize = 4f;

        [SerializeField] Vector3 hideOffset;

        [SerializeField, Range(float.Epsilon, 0.5f),]
        float hideSize;

        [SerializeField, Range(0.1f, 2f),] float sizeMulti = 1f;
        public float currentSize;
        float fluidFactor = 0.8f;
        float fluidMax = 100;
        bool hidden;

        void HideBalls() {
            if (hidden)
                return;
            transform.localScale = new Vector3(hideSize, hideSize, hideSize);
            transform.localPosition += hideOffset;
            hidden = true;
        }

        public void ShowOrHide(bool show) {
            if (show)
                ShowBalls();
            else
                HideBalls();
        }

        void ShowBalls() {
            if (!hidden)
                return;
            hidden = false;
            transform.localPosition -= hideOffset;
            ReSize(currentSize);
        }

        public void ReSize(float newSize) {
            var size = Mathf.Clamp(newSize, minSize, maxSize);
            currentSize = size;
            SetBallSize();
        }

        void SetBallSize() {
            var finalSize = currentSize * fluidFactor * sizeMulti;
            transform.localScale = new Vector3(finalSize, finalSize, finalSize);
        }

        public void SetupFluidStretch(BaseOrgansContainer container) {
            fluidMax = container.Fluid.Value;
            SetFluidStretch(container.Fluid.CurrentValue);
        }

        public void SetFluidStretch(float current) => fluidFactor = Mathf.Clamp(current / fluidMax, 0.8f, 1.1f);
    }
}