using Safe_To_Share.Scripts.Holders;
using UnityEngine;
using UnityEngine.UI;

namespace Safe_To_Share.Scripts.GameUIAndMenus.InteractiveActiveAilments
{
    public abstract class DoThingButton : MonoBehaviour
    {
        [SerializeField] float threesHold = 0.4f;
        [SerializeField] Color start, end;
        [SerializeField] protected PlayerHolder holder;
        [SerializeField] Image backGround;
        public void Setup(PlayerHolder playerHolder) => holder = playerHolder;
#if UNITY_EDITOR
        
        void OnValidate()
        {
            if (backGround != null) 
                backGround.color = start;
        }
#endif

        public void ValueChange(float pressure)
        {
            if (pressure < threesHold)
                gameObject.SetActive(false);
            else
            {
                gameObject.SetActive(true);
                var percent = (pressure - threesHold) / (1f - threesHold);
                var newColor = Color.Lerp(start, end, percent);
                backGround.color = newColor;
            }
        }

        public abstract void OnClick();
    }
}