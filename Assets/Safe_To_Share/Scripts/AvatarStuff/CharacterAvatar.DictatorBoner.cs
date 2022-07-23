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
            [SerializeField] int flaccid;
            [SerializeField] int boner;
            [SerializeField] DictatorDick dictatorDick;
            [SerializeField] DictatorBalls dictatorBalls;
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
                foreach (var meshRenderer in skinnedMeshRenderers)
                {
                    var rendererMaterials = meshRenderer.materials;
                    for (int index = 0; index < rendererMaterials.Length; index++)
                        foreach (Material dickMat in dickMats)
                            if (rendererMaterials[index].name.Contains(dickMat.name))
                            {
                                rendererMaterials[index] = invisible;
                                break;
                            }

                    meshRenderer.materials = rendererMaterials;
                }
            }
        }
    }
}