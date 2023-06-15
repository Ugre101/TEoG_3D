using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Character {
    [Serializable]
    public class BodyMorphs {
        [SerializeField] List<AvatarBodyMorphs> avatarBodyMorphs = new();

        Dictionary<string, AvatarBodyMorphs> dict;
        public Dictionary<string, AvatarBodyMorphs> Dict => dict ??= avatarBodyMorphs.ToDictionary(a => a.avatarGuid);

        public void Loaded() => dict = null;

        public void AddLimited(string avatarGuid, AvatarBodyMorphs.BodyMorph morph) {
            if (avatarBodyMorphs.Exists(ab => ab.avatarGuid == avatarGuid)) {
                avatarBodyMorphs.Find(ab => ab.avatarGuid == avatarGuid).bodyAvatarMorphs.Add(morph);
            } else // TODO Test if working
            {
                avatarBodyMorphs.Add(new AvatarBodyMorphs(avatarGuid, new List<AvatarBodyMorphs.BodyMorph> { morph, }));
                dict = null;
            }
        }

        public AvatarBodyMorphs AddNew(string avatarGuid, IEnumerable<AvatarBodyMorphs.BodyMorph> avatar) {
            var list = avatar.ToList();
            var morphs = new AvatarBodyMorphs(avatarGuid, list);
            avatarBodyMorphs.Add(morphs);
            dict = null;
            return morphs;
        }


        [Serializable]
        public class AvatarBodyMorphs {
            public string avatarGuid;
            public List<BodyMorph> bodyAvatarMorphs;

            public AvatarBodyMorphs(string avatarGuid, List<BodyMorph> avatarMorphs) {
                this.avatarGuid = avatarGuid;
                bodyAvatarMorphs = avatarMorphs;
            }

            [Serializable]
            public class BodyMorph {
                public string title;
                public float value;

                public BodyMorph(string title) => this.title = title;

                public BodyMorph(string title, float value) {
                    this.title = title;
                    this.value = value;
                }
            }
        }
    }
}