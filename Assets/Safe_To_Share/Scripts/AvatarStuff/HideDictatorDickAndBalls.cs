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

        readonly Dictionary<SkinnedMeshRenderer, List<KeyValuePair<int, Material>>> dickMatsDict = new();
        bool ballsHidden;

        readonly Dictionary<SkinnedMeshRenderer, int> ballsIndexDict = new();
        bool dickHidden;

        bool hasMatches;

        void HideDick(IEnumerable<SkinnedMeshRenderer> skinnedMeshRenderers)
        {
            dickHidden = true;
            var meshRenderers = skinnedMeshRenderers.ToList();
            if (!hasMatches)
                FindMats(meshRenderers);

            foreach (var meshRenderer in meshRenderers)
            {
                var rendererMaterials = meshRenderer.materials;
                if (dickMatsDict.TryGetValue(meshRenderer, out var valuePairs))
                    foreach ((var key, var value) in valuePairs)
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
                foreach (var targetMat in targetMats)
                {
                    var index = -1;
                    for (var i = 0; i < mats.Length; i++)
                    {
                        var material = mats[i];
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
                var rendererMaterials = meshRenderer.materials;
                if (dickMatsDict.TryGetValue(meshRenderer, out var pairs))
                    foreach ((var key, var value) in pairs)
                        rendererMaterials[key] = value;
                meshRenderer.materials = rendererMaterials;
            }
        }

        /// <returns>If skin color need to be updated</returns>
        public bool Handle(IEnumerable<SkinnedMeshRenderer> skinnedMeshRenderers, bool hasDick, bool hasBalls)
        {
            var update = false;
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
            ballsHidden = false;
            foreach (var meshRenderer in skinnedMeshRenderers)
                if (ballsIndexDict.TryGetValue(meshRenderer, out var ballsIndex))
                {
                    var rendererMaterials = meshRenderer.materials;
                    rendererMaterials[ballsIndex] = ballsTargetMat;
                    meshRenderer.materials = rendererMaterials;
                }
        }

        void HideBalls(IEnumerable<SkinnedMeshRenderer> skinnedMeshRenderers)
        {
            ballsHidden = true;
            foreach (var meshRenderer in skinnedMeshRenderers)
            {
                var rendererMaterials = meshRenderer.materials;
                if (ballsIndexDict.TryGetValue(meshRenderer, out var ballsIndex))
                {
                    rendererMaterials[ballsIndex] = invisibleMat;
                    meshRenderer.materials = rendererMaterials;
                }
                else
                {
                    for (var index = 0; index < rendererMaterials.Length; index++)
                        if (rendererMaterials[index].name.Contains(ballsTargetMat.name))
                        {
                            rendererMaterials[index] = invisibleMat;
                            ballsIndexDict.TryAdd(meshRenderer, index);
                        }

                    meshRenderer.materials = rendererMaterials;
                }
            }
        }
    }
}