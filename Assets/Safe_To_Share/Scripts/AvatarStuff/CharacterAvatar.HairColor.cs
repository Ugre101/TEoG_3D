using System;
using System.Collections.Generic;
using System.Linq;
using Character;
using UnityEngine;

namespace AvatarStuff {
    public partial class CharacterAvatar {
        [Serializable]
        public class HairColor {
            [SerializeField] List<Material> skip = new();

            public void SetHairColor(IEnumerable<SkinnedMeshRenderer> hairRenders, Hair hair) {
                foreach (var render in hairRenders)
                foreach (var hairMat in render.materials)
                    if (!skip.Any(s => hairMat.name.Contains(s.name)))
                        hairMat.color = hair.HairColor;
            }

            public void SetBald(IEnumerable<SkinnedMeshRenderer> hairRenders, bool bald) {
                foreach (var hairRender in hairRenders)
                    hairRender.gameObject.SetActive(!bald);
            }
        }
    }
}