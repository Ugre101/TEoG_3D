using System.Collections.Generic;
using UnityEngine;

namespace AvatarStuff
{
    public sealed class DazDickController : MonoBehaviour
    {
        [SerializeField] Transform[] genitals;
        [SerializeField, Range(0.1f, 0.5f),] float downBend = 0.1f;
        [SerializeField, Min(0.01f),] float dickMin, dickMax = 3f;
        [SerializeField] Vector3 hideOffset = new(0, -0.1f, -0.05f);
        [SerializeField] float hideSize = 1f;
        [SerializeField, HideInInspector,] bool hasGenitals;
        [SerializeField, HideInInspector,] bool hasShapes;

        [Header("Boner Morph"), SerializeField,]
        List<SkinnedMeshRenderer> bodyShapes = new();

        [SerializeField] int bonerId;
        bool hidden;

        float DickSize { get; set; }

        /// <summary>0 No boner => 1 Max boner</summary>
        float Boner { get; set; }

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

        public void SetDickSize(float value)
        {
            DickSize = Mathf.Clamp(value, dickMin, dickMax);
            ScaleDick();
        }

        void ScaleDick()
        {
            if (!hasGenitals) return;
            float bonerMod = Mathf.Clamp(0.6f + Boner * 0.5f, 0.7f, 1.1f);
            float dickSize = DickSize * bonerMod;
            genitals[0].localScale = new Vector3(dickSize, dickSize, dickSize);
        }

        public void SetBoner(float value)
        {
            Boner = Mathf.Clamp(value / 70f, 0f, 1f);
            if (!hasGenitals)
                return;
            ScaleDick();
            if (hasShapes)
                foreach (SkinnedMeshRenderer meshRenderer in bodyShapes)
                    meshRenderer.SetBlendShapeWeight(bonerId, Mathf.Clamp(100f - value * 1.5f, 0f, 100f));
            else
                for (int i = 0; i < genitals.Length; i++)
                    genitals[i].localEulerAngles = GenBend(i);
        }

        Vector3 GenBend(int gen) => new(-Boner * 30 * (1 - downBend * gen), 0, 0);
#if UNITY_EDITOR
        [ContextMenu("Quick setup")]
        public void QuickSetup() => genitals = GetComponentsInChildren<Transform>();

        void OnValidate()
        {
            genitals = GetComponentsInChildren<Transform>();
            hasGenitals = genitals is { Length: > 0, };
            hasShapes = bodyShapes is { Count: > 0, };
        }
#endif
    }
}