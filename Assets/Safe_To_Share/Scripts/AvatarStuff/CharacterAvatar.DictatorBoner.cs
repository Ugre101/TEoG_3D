using System;
using System.Collections.Generic;
using UnityEngine;

namespace AvatarStuff
{
    public partial class CharacterAvatar
    {
        [Serializable]
        public struct DictatorBoner
        {
            [SerializeField] bool hasDictatorDick;
            [SerializeField] bool hasDictatorBalls;
            [SerializeField] bool hasHide;
            [SerializeField] int flaccid;
            [SerializeField] int boner;
            [SerializeField] DictatorDick dictatorDick;
            [SerializeField] DictatorBalls dictatorBalls;
            [SerializeField] HideDictatorDickAndBalls hideDictator;
            [Header("Hide"), SerializeField,] Material[] dickMats;
            [SerializeField] Material invisible;

            public void ChangeShape(IEnumerable<SkinnedMeshRenderer> shapes, int arousal)
            {
                if (!hasDictatorDick)
                    return;
                float flaccidValue = Mathf.Clamp(100f - arousal * 1.6f, 0f, 100f);
                float bonerValue = Mathf.Clamp(arousal - 60f, 0f, 25f);
                foreach (SkinnedMeshRenderer shape in shapes)
                {
                    shape.SetBlendShapeWeight(flaccid, flaccidValue);
                    shape.SetBlendShapeWeight(boner, bonerValue);
                }

                dictatorDick.SetBoner(arousal);
            }

            public void SetDickSize(float dicksBiggest)
            {
                if (!hasDictatorDick)
                    return;
                dictatorDick.SetDickSize(dicksBiggest);
            }

            public void SetBallsSize(float biggestBalls)
            {
                if (!hasDictatorBalls)
                    return;
                dictatorBalls.ReSize(biggestBalls);
            }

            public void UpdateBallsFluid(float biggestBalls)
            {
                if (!hasDictatorBalls)
                    return;
                dictatorBalls.ReSize(biggestBalls);
            }

            public void HideOrShow(bool hasDick, List<SkinnedMeshRenderer> skinnedMeshRenderers)
            {
                if (!hasDictatorDick)
                    return;
                dictatorDick.HideOrShow(hasDick);
            }

            public void OnValidate()
            {
                hasHide = hideDictator != null;
                hasDictatorBalls = dictatorBalls != null;
                hasDictatorDick = dictatorDick != null;
            }
            public bool Handle(IEnumerable<SkinnedMeshRenderer> skinnedMeshRenderers, bool hasDick, bool hasBalls) => hasHide && hideDictator.Handle(skinnedMeshRenderers, hasDick, hasBalls);
        }
    }
}