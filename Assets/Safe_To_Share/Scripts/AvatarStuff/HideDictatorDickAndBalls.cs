using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AvatarStuff
{
    public class HideDictatorDickAndBalls : MonoBehaviour
    {
        [SerializeField] Material[] targetMats;
        [SerializeField] Material ballsTargetMat;
        [SerializeField] Material invisibleMat;
        bool ballsHidden;

        int? ballsIndex;
        bool dickHidden;

        bool hasMatches;

        readonly Dictionary<SkinnedMeshRenderer, List<KeyValuePair<int, Material>>> dickMatsDict = new();

        void HideDick(IEnumerable<SkinnedMeshRenderer> skinnedMeshRenderers)
        {
            dickHidden = true;
            var meshRenderers = skinnedMeshRenderers.ToList();
            if (!hasMatches) 
                FindMats(meshRenderers);

            foreach (var meshRenderer in meshRenderers)
            {
                var rendererMaterials = meshRenderer.materials;
                if(dickMatsDict.TryGetValue(meshRenderer,out var valuePairs))
                    foreach ((int key, Material value) in valuePairs)
                        rendererMaterials[key] = invisibleMat;
                meshRenderer.materials = rendererMaterials;
            }
        }

        void FindMats(List<SkinnedMeshRenderer> meshRenderers)
        {
            foreach (var meshRenderer in meshRenderers)
            {
                dickMatsDict.Add(meshRenderer, new List<KeyValuePair<int, Material>>());
                var mats = meshRenderer.materials;
                foreach (Material targetMat in targetMats)
                {
                    int index = -1;
                    for (int i = 0; i < mats.Length; i++)
                    {
                        Material material = mats[i];
                        if (material.name.Contains(targetMat.name)) 
                            index = i;
                    }

                    if (-1 >= index) continue;
                    if (dickMatsDict.TryGetValue(meshRenderer, out var list))
                        list.Add(new KeyValuePair<int, Material>(index, targetMat));
                }
            } //var mats = meshRenderers.First().materials.ToList();
            hasMatches = true;
        }

        void ShowDick(IEnumerable<SkinnedMeshRenderer> skinnedMeshRenderers)
        {
            dickHidden = false;
            if (hasMatches is false) return;

            foreach (var meshRenderer in skinnedMeshRenderers)
            {
                Material[] rendererMaterials = meshRenderer.materials;
                if (dickMatsDict.TryGetValue(meshRenderer, out var pairs))
                    foreach ((int key, Material value) in pairs)
                        rendererMaterials[key] = value;
                meshRenderer.materials = rendererMaterials;
            }
        }

        /// <returns>If skin color need to be updated</returns>
        public bool Handle(IEnumerable<SkinnedMeshRenderer> skinnedMeshRenderers, bool hasDick, bool hasBalls)
        {
            bool update = false;
            var meshRenderers = skinnedMeshRenderers.ToList();
            if (dickHidden && hasDick)
            {
                ShowDick(meshRenderers);
                update = true;
            }

            if (ballsHidden && hasBalls)
            {
                ShowBalls(meshRenderers);
                update = true;
            }

            if (!ballsHidden && !hasBalls)
                HideBalls(meshRenderers);
            if (!dickHidden && !hasDick)
                HideDick(meshRenderers);
            return update;
        }

        void ShowBalls(IEnumerable<SkinnedMeshRenderer> skinnedMeshRenderers)
        {
            if (ballsIndex.HasValue is false) return;
            ballsHidden = false;
            foreach (var meshRenderer in skinnedMeshRenderers)
            {
                Material[] rendererMaterials = meshRenderer.materials;
                rendererMaterials[ballsIndex.Value] = ballsTargetMat;
                meshRenderer.materials = rendererMaterials;
            }
        }

        void HideBalls(IEnumerable<SkinnedMeshRenderer> skinnedMeshRenderers)
        {
            ballsHidden = true;
            if (ballsIndex.HasValue)
                foreach (var meshRenderer in skinnedMeshRenderers)
                {
                    var rendererMaterials = meshRenderer.materials;
                    rendererMaterials[ballsIndex.Value] = invisibleMat;
                    meshRenderer.materials = rendererMaterials;
                }
            else
                foreach (var meshRenderer in skinnedMeshRenderers)
                {
                    var rendererMaterials = meshRenderer.materials;
                    for (int index = 0; index < rendererMaterials.Length; index++)
                        if (rendererMaterials[index].name.Contains(ballsTargetMat.name))
                        {
                            ballsIndex = index;
                            rendererMaterials[index] = invisibleMat;
                        }
                    meshRenderer.materials = rendererMaterials;
                }
        }
    }
}