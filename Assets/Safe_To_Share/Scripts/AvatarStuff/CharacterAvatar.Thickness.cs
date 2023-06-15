using System;
using Character.BodyStuff.BodyBuild;
using UnityEngine;

namespace AvatarStuff {
    public partial class CharacterAvatar {
        [Serializable]
        public struct Thickness {
            [SerializeField] bool hasShape;
            [SerializeField] int thick;
            [SerializeField] int thin;

            public void ChangeShape(SkinnedMeshRenderer shape, Thickset thickset) {
                if (!hasShape)
                    return;
                var clampedValue = Mathf.Clamp(Mathf.Abs(thickset.Value) * 30f, 0, 300f);
                if (thickset.Value > 0) {
                    shape.SetBlendShapeWeight(thick, clampedValue);
                    shape.SetBlendShapeWeight(thin, 0f);
                } else {
                    shape.SetBlendShapeWeight(thick, 0);
                    shape.SetBlendShapeWeight(thin, clampedValue);
                }
            }

            public void EditorQuickAddThin(int i) => thin = i;

            public void EditorQuickAddThick(int i) => thick = i;
        }
    }
}