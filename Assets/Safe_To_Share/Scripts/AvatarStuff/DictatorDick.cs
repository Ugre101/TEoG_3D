using UnityEngine;

namespace AvatarStuff
{
    public class DictatorDick : MonoBehaviour
    {
        [SerializeField] Transform[] genitals;
        [SerializeField,Min(0.01f),] float dickMin, dickMax = 2.5f;
        [SerializeField] Vector3 hideOffset;
        [SerializeField,Range(float.Epsilon,0.2f)] float hideSize = 1f;
        [SerializeField,HideInInspector] bool hasGenitals;
        [Header("Hide dick")]
        bool hidden;
        void HideDick()
        {
            if (hidden) 
                return;
            transform.localPosition += hideOffset;
            genitals[0].localScale = new Vector3(hideSize, hideSize, hideSize);
            hidden = true;
        }

        public void HideOrShow(bool show)
        {
            if (show)
                ShowDick();
            else 
                HideDick();
        }
        void ShowDick()
        {
            if (!hidden)
                return;
            transform.localPosition -= hideOffset;
            hidden = false;
        }

        float DickSize { get; set; }

        public void SetDickSize(float value)
        {
            DickSize = Mathf.Clamp(value / 3f, dickMin, dickMax);
            ScaleDick();
        }

        void ScaleDick()
        {
            if (!hasGenitals) return;
            float bonerMod = Mathf.Clamp(0.6f + Boner * 0.5f, 0.7f, 1f);
            float dickSize = DickSize * bonerMod;
            genitals[0].localScale = new Vector3(dickSize, dickSize, dickSize);
        }


        float Boner { get; set; } = 1f;
        public void SetBoner(float value)
        {
            Boner = Mathf.Clamp(value / 70f, 0f, 1f);
            if (!hasGenitals)
                return;
            ScaleDick();
        }
#if UNITY_EDITOR

        void OnValidate()
        {
            genitals = GetComponentsInChildren<Transform>();
            hasGenitals = genitals != null && genitals.Length > 0;
        }
#endif
       
    }
}