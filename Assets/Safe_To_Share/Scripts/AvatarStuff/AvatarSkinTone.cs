using System;
using System.Collections.Generic;
using UnityEngine;

namespace AvatarStuff {
    [Serializable]
    public class AvatarSkinTone {
        [SerializeField] Color lightest = Color.white, darkest;

        [SerializeField] Material[] skinMats;

        float current;
        public Color Darkest => darkest;

        public Color Lightest => lightest;

        public void SetSkinTone(float value, List<SkinnedMeshRenderer> renderers, bool forceUpdate) {
            if (!forceUpdate && Math.Abs(value - current) < 0.01f)
                return;
            current = value;
            var tone = Color.Lerp(Lightest, Darkest, value);

            foreach (var meshRenderer in renderers)
            foreach (var rendererMaterial in meshRenderer.materials)
            foreach (var skinMat in skinMats)
                if (rendererMaterial.name.Contains(skinMat.name)) {
                    rendererMaterial.color = tone;
                    break;
                }
        }
    }
}