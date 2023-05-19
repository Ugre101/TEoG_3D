using System.Collections.Generic;
using UnityEngine;

namespace AvatarStuff
{
    public sealed class TurnAvatarDoll : MonoBehaviour
    {
        [SerializeField] Material vaginaTarget;
        [SerializeField] Material dollMat;

        bool turned;
        public bool HandleTurnDool(IEnumerable<SkinnedMeshRenderer> skinnedMeshRenderers, bool turn)
        {
            if (turned == turn)
                return false;
            TurnDoll(skinnedMeshRenderers,turn);
            turned = turn;
            return true;
        }

        void TurnDoll(IEnumerable<SkinnedMeshRenderer> skinnedMeshRenderers, bool turn)
        {
            foreach (var meshRenderer in skinnedMeshRenderers)
            {
                var rendererMaterials = meshRenderer.materials;
                for (var index = 0; index < rendererMaterials.Length; index++)
                    if (rendererMaterials[index].name.Contains(turn ? vaginaTarget.name : dollMat.name))
                        rendererMaterials[index] = turn ? dollMat : vaginaTarget;
                meshRenderer.materials = rendererMaterials;
            }
        }
    }
}