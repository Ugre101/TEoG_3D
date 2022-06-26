using System;
using Character.BodyStuff.BodyBuild;
using UnityEngine;

namespace AvatarStuff
{
    public partial class CharacterAvatar
    {
        [Serializable]
        public struct Thickness
        {
            [SerializeField] bool hasShape;
            [SerializeField] int thick;
            [SerializeField] int thin;
            public void ChangeShape(SkinnedMeshRenderer shape, Thickset thickset)
            {
                if (!hasShape)
                    return;
                if (thickset.Value > 0)
                {
                    shape.SetBlendShapeWeight(thick, thickset.Value * 100f);
                    shape.SetBlendShapeWeight(thin, 0f);
                }
                else
                {
                    shape.SetBlendShapeWeight(thick, 0);
                    shape.SetBlendShapeWeight(thin,  Mathf.Abs(thickset.Value * 100f));
                }
            }

            public void EditorQuickAddThin(int i) => thin = i;

            public void EditorQuickAddThick(int i) => thick = i;
        }
    }
}