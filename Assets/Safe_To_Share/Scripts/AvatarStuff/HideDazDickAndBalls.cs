using System.Collections.Generic;
using UnityEngine;

namespace AvatarStuff
{
    public sealed class HideDazDickAndBalls : MonoBehaviour
    {
        [SerializeField] Material targetMat;
        [SerializeField] Material invisibleMat;

        bool hidden;

        void HideDick(IEnumerable<SkinnedMeshRenderer> skinnedMeshRenderers, bool hide)
        {
            hidden = hide;
            foreach (var meshRenderer in skinnedMeshRenderers)
            {
                var rendererMaterials = meshRenderer.materials;
                for (var index = 0; index < rendererMaterials.Length; index++)
                    if (rendererMaterials[index].name.Contains(hide ? targetMat.name : invisibleMat.name))
                        rendererMaterials[index] = hide ? invisibleMat : targetMat;
                meshRenderer.materials = rendererMaterials;
            }
        }

        /// <returns>If skin color need to be updated</returns>
        public bool Handle(IEnumerable<SkinnedMeshRenderer> skinnedMeshRenderers, bool hasDick, bool hasBalls)
        {
            switch (hidden)
            {
                case true when !hasDick && !hasBalls:
                    return false;
                case true:
                    HideDick(skinnedMeshRenderers, false);
                    return true;
                case false when hasBalls || hasDick:
                    return false;
            }

            HideDick(skinnedMeshRenderers, true);
            return false;
        }
    }
}