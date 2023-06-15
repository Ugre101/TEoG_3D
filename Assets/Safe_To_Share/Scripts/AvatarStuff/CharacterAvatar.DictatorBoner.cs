using System;
using System.Collections.Generic;
using Character.Organs.OrgansContainers;
using UnityEngine;

namespace AvatarStuff {
    public partial class CharacterAvatar {
        [Serializable]
        public struct DictatorBoner {
            [field: SerializeField] public bool hasDictatorDick { get; private set; }
            [field: SerializeField] public bool hasDictatorBalls { get; private set; }
            [SerializeField] bool hasHide;
            [SerializeField] int flaccid;
            [SerializeField] int boner;
            [SerializeField] DictatorDick dictatorDick;
            [SerializeField] DictatorBalls dictatorBalls;
            [SerializeField] HideDictatorDickAndBalls hideDictator;
            [Header("Hide"), SerializeField,] Material[] dickMats;
            [SerializeField] Material invisible;

            public void ChangeShape(IEnumerable<SkinnedMeshRenderer> shapes, int arousal) {
                if (!hasDictatorDick)
                    return;
                var flaccidValue = Mathf.Clamp(100f - arousal * 1.6f, 0f, 100f);
                var bonerValue = Mathf.Clamp(arousal - 60f, 0f, 25f);
                foreach (var shape in shapes) {
                    shape.SetBlendShapeWeight(flaccid, flaccidValue);
                    shape.SetBlendShapeWeight(boner, bonerValue);
                }

                dictatorDick.SetBoner(arousal);
            }

            public void SetDickSize(float dicksBiggest) {
                if (!hasDictatorDick)
                    return;
                dictatorDick.SetDickSize(dicksBiggest);
            }

            public void SetBallsSize(float biggestBalls) {
                if (!hasDictatorBalls)
                    return;
                dictatorBalls.ReSize(biggestBalls);
            }


            public void SetupFluidStretch(BaseOrgansContainer container) => dictatorBalls.SetupFluidStretch(container);

            public void SetFluidStretch(float current) => dictatorBalls.SetFluidStretch(current);

            public void HideOrShowDick(bool hasDick) {
                if (!hasDictatorDick)
                    return;
                dictatorDick.HideOrShow(hasDick);
            }

            public void OnValidate() {
                hasHide = hideDictator != null;
                hasDictatorBalls = dictatorBalls != null;
                hasDictatorDick = dictatorDick != null;
            }

            public bool HandleHideDickAndBalls(IEnumerable<SkinnedMeshRenderer> skinnedMeshRenderers, bool hasDick,
                                               bool hasBalls) =>
                hasHide && hideDictator.Handle(skinnedMeshRenderers, hasDick, hasBalls);
        }
    }
}