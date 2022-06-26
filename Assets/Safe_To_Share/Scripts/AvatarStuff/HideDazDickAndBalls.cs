using System.Collections.Generic;
using UnityEngine;

namespace AvatarStuff
{
    public class HideDazDickAndBalls : MonoBehaviour
    {
        [SerializeField] Material targetMat;
        [SerializeField] Material invisibleMat;

        bool hidden;
        
        void Hide(IEnumerable<SkinnedMeshRenderer> skinnedMeshRenderers)
        {
            hidden = true;
            foreach (SkinnedMeshRenderer meshRenderer in skinnedMeshRenderers)
            {
                var rendererMaterials = meshRenderer.materials;
                for (int index = 0; index < rendererMaterials.Length; index++)
                    if (rendererMaterials[index].name.Contains(targetMat.name))
                        rendererMaterials[index] = invisibleMat;
                meshRenderer.materials = rendererMaterials;
            }
        }

        void Show(IEnumerable<SkinnedMeshRenderer> skinnedMeshRenderers)
        {
            hidden = false;
            foreach (SkinnedMeshRenderer meshRenderer in skinnedMeshRenderers)
            {
                Material[] rendererMaterials = meshRenderer.materials;
                for (int index = 0; index < rendererMaterials.Length; index++)
                    if (rendererMaterials[index].name.Contains(invisibleMat.name))
                        rendererMaterials[index] = targetMat;
                meshRenderer.materials = rendererMaterials;
            }
        }

        public void Handle(IEnumerable<SkinnedMeshRenderer> skinnedMeshRenderers, bool hasDick, bool hasBalls)
        {
            if (hidden)
            {
                if (hasDick || hasBalls) 
                    Show(skinnedMeshRenderers);
            }
            else if (!hasDick && !hasBalls)
                Hide(skinnedMeshRenderers);

        }
    }
}