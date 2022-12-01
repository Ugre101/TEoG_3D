using Safe_To_Share.Scripts.Holders;
using UnityEngine;
using UnityEngine.UI;

namespace Safe_To_Share.Scripts.GameUIAndMenus.InteractiveActiveAilments
{
    public abstract class DoThingButton : MonoBehaviour
    {
        [SerializeField] Color start, end;
        [SerializeField] protected PlayerHolder holder;
        [SerializeField] Image backGround;
        protected abstract float ThreesHold { get; }
        protected abstract bool Enabled { get; }
#if UNITY_EDITOR

        void OnValidate()
        {
            if (backGround != null)
                backGround.color = start;
        }
#endif
        public void Setup(PlayerHolder playerHolder) => holder = playerHolder;

        public void ValueChange(float pressure)
        {
            if (Enabled is false)
                gameObject.SetActive(false);
            else if (pressure < ThreesHold)
                gameObject.SetActive(false);
            else
            {
                gameObject.SetActive(true);
                var percent = (pressure - ThreesHold) / (1f - ThreesHold);
                var newColor = Color.Lerp(start, end, percent);
                backGround.color = newColor;
            }
        }

        public abstract void OnClick();
    }
}