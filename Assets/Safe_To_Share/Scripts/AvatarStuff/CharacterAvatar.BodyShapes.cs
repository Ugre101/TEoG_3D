using System;
using System.Collections.Generic;
using System.Linq;
using Character;
using Safe_To_Share.Scripts.Static;
using UnityEngine;
using Random = UnityEngine.Random;

namespace AvatarStuff {
    public partial class CharacterAvatar {
        [SerializeField] BodyShapes bodyShapes;
        public BodyShapes AvatarBodyShapes => bodyShapes;

        public void UpdateBodyTypeMorphs(BodyMorphs bodyMorphs) {
            if (!bodyMorphs.Dict.TryGetValue(prefab.AssetGUID, out var match)) return;
            AvatarBodyShapes.UpdateBodyMorphs(AllShapes, match);
        }

        public void UpdateABodyTypeMorphs(BodyMorphs.AvatarBodyMorphs.BodyMorph myMorph) =>
            AvatarBodyShapes.UpdateOneShape(AllShapes, myMorph);

        public void GetRandomBodyMorphs(BaseCharacter character) {
            var bodyShapesOfTypeBody = AvatarBodyShapes.GetBodyShapesOfType(BodyShapes.BodyShapeTypes.Body).ToArray();
            if (!bodyShapesOfTypeBody.Any()) return;
            var bodyShapesOfTypeFace = AvatarBodyShapes.GetBodyShapesOfType(BodyShapes.BodyShapeTypes.Face).ToArray();
            if (!bodyShapesOfTypeFace.Any()) return;
            var body = bodyShapesOfTypeBody[Random.Range(0, bodyShapesOfTypeBody.Length)];
            var min = 999;
            BodyShapes.BodyShape? hit = null;
            foreach (var faceShape in bodyShapesOfTypeFace) {
                var res = UgreTools.Levenshtein.Compute(body.Title, faceShape.Title);
                if (res < min) {
                    hit = faceShape;
                    min = res;
                }
            }

            if (hit.HasValue) {
                var range = Random.Range(30, 100);
                character.Body.Morphs.AddLimited(prefab.AssetGUID,
                    new BodyMorphs.AvatarBodyMorphs.BodyMorph(body.Title, range));
                character.Body.Morphs.AddLimited(prefab.AssetGUID,
                    new BodyMorphs.AvatarBodyMorphs.BodyMorph(hit.Value.Title, range));
            }
        }

        [Serializable]
        public struct BodyShapes {
            public enum BodyShapeTypes {
                Body, Face,
            }

            [SerializeField] List<BodyShape> shapes;

            Dictionary<string, BodyShape> bodyShapesDict;

            Dictionary<string, BodyShape> BodyShapesDict => bodyShapesDict ??= shapes.ToDictionary(s => s.Title);

            public void UpdateBodyMorphs(IEnumerable<SkinnedMeshRenderer> renderers,
                                         BodyMorphs.AvatarBodyMorphs bodyMorphs) {
                var meshRenderers = renderers as SkinnedMeshRenderer[] ?? renderers.ToArray();
                foreach (var subStruct in bodyMorphs.bodyAvatarMorphs)
                    if (BodyShapesDict.TryGetValue(subStruct.title, out var bodyShape))
                        foreach (var meshRenderer in meshRenderers)
                            meshRenderer.SetBlendShapeWeight(bodyShape.ID, subStruct.value);
            }

            public void UpdateOneShape(IEnumerable<SkinnedMeshRenderer> renderers,
                                       BodyMorphs.AvatarBodyMorphs.BodyMorph bodyMorph) {
                if (!BodyShapesDict.TryGetValue(bodyMorph.title, out var bodyShape)) return;
                foreach (var meshRenderer in renderers)
                    meshRenderer.SetBlendShapeWeight(bodyShape.ID, bodyMorph.value);
            }

            public IEnumerable<BodyMorphs.AvatarBodyMorphs.BodyMorph> AddToCharacter() =>
                shapes.Select(bodyShape => new BodyMorphs.AvatarBodyMorphs.BodyMorph(bodyShape.Title));

            public IEnumerable<BodyShape> GetBodyShapesOfType(BodyShapeTypes type) =>
                shapes.Where(bodyShape => bodyShape.Type == type);

            [Serializable]
            public struct BodyShape {
                [SerializeField] int id;
                [SerializeField] string title;
                [SerializeField] BodyShapeTypes type;

                public BodyShape(int id, string title, BodyShapeTypes type) {
                    this.id = id;
                    this.title = title;
                    this.type = type;
                }

                public int ID => id;

                public string Title => title;

                public BodyShapeTypes Type => type;
            }
#if UNITY_EDITOR
            public bool EditorQuickSetup(string shapeName, int id) {
                if (shapes.Exists(s => s.ID == id))
                    return true;
                if (shapeName.Contains("body")) {
                    if (!shapes.Exists(s => s.ID == id))
                        AddShape(shapeName, id, BodyShapeTypes.Body);
                    return true;
                }

                if (shapeName.Contains("face") || shapeName.Contains("head")) {
                    if (!shapes.Exists(s => s.ID == id))
                        AddShape(shapeName, id, BodyShapeTypes.Face);
                    return true;
                }

                if (shapeName.Contains("fhm")) {
                    // Head
                    if (!shapes.Exists(s => s.ID == id))
                        AddShape(shapeName, id, BodyShapeTypes.Face);
                    return true;
                }

                if (shapeName.Contains("fbm")) {
                    // Body
                    if (!shapes.Exists(s => s.ID == id))
                        AddShape(shapeName, id, BodyShapeTypes.Body);
                    return true;
                }

                return false;
            }

            void AddShape(string shapeName, int id, BodyShapeTypes type) {
                var index = shapeName.LastIndexOf("male_", StringComparison.Ordinal);
                var cleanedName = shapeName.Substring(index + 5);
                shapes.Add(new BodyShape(id, cleanedName, type));
            }
#endif
        }
    }
}