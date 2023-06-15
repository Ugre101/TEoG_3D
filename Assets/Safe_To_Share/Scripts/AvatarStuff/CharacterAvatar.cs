﻿using System;
using System.Collections.Generic;
using System.Linq;
using Character;
using Character.BodyStuff;
using Character.GenderStuff;
using Character.Organs;
using Character.Organs.OrgansContainers;
using Character.PregnancyStuff;
using Character.Race.Races;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace AvatarStuff {
    [RequireComponent(typeof(FootSteps))]
    public partial class CharacterAvatar : MonoBehaviour {
        const float OrganMulti = 0.7f;
        [SerializeField] Gender[] supportedGenders;
        [SerializeField] BasicRace[] supportedRaces;
        [SerializeField] AssetReference prefab;

        [SerializeField] Material[] hairMats;
        [SerializeField] AvatarSkinTone skinTone;

        [SerializeField] HideDazDickAndBalls hideDazDickAndBalls;
        [SerializeField] DazDickController dickController;
        [SerializeField] DazBallsController ballsController;
        [SerializeField] protected List<SkinnedMeshRenderer> bodyMeshRenderers;
        [SerializeField] protected List<SkinnedMeshRenderer> detailsMeshRenderers;
        [SerializeField] protected List<SkinnedMeshRenderer> hairMeshRenderers;

        [Header("Body Morphs"), SerializeField,]
        BlendShape bodyFat;

        [SerializeField] BlendShape noBoobs;
        [SerializeField] BlendShape biggerBoobs;
        [SerializeField] BlendShape pregnant;
        [SerializeField] BlendShape bodyMuscle;
        [SerializeField] Thickness thickness;
        [SerializeField, Range(50f, 500f),] float fatMultiplier = 200f;

        [Header("Vore"), SerializeField,] VoreShapes vore = new();

        [Header("Hair"), SerializeField,] bool changeHairColor = true;
        [SerializeField] HairColor hairColor = new();
        [SerializeField] DictatorBoner dictatorBoner;

        [Header("Dont touch"), SerializeField,]
        bool hasBallsController;

        [SerializeField] bool hasDickController;
        [SerializeField] bool hasHideDaz;
        [field: SerializeField] public Animator Animator { get; private set; }

        [field: SerializeField] public AvatarKeyAreas KeyAreas { get; private set; }

        float ballSize;
        bool firstUse = true;

        bool hasShapes;
        float lastTick;
        SkinnedMeshRenderer[] shapes;

        public SkinnedMeshRenderer[] AllShapes {
            get {
                if (hasShapes)
                    return shapes;
                var temp = bodyMeshRenderers.Concat(hairMeshRenderers);
                temp = temp.Concat(detailsMeshRenderers);
                shapes = temp.ToArray();
                hasShapes = true;
                return shapes;
            }
        }

        public IEnumerable<Gender> SupportedGenders => supportedGenders;
        public IEnumerable<BasicRace> SupportedRaces => supportedRaces;

        public AssetReference Prefab => prefab;

        public Material[] HairMats => hairMats;

        void Update() {
            if (firstUse || Time.time < lastTick + 1f)
                return;
            lastTick = Time.time;
            vore.TickVoreStruggle(ballsController);
        }

#if UNITY_EDITOR
        void OnValidate() {
            hasDickController = dickController != null;
            hasBallsController = ballsController != null;
            hasHideDaz = hideDazDickAndBalls != null;
            dictatorBoner.OnValidate();
            if (Application.isPlaying) return;
            if (bodyMeshRenderers is not { Count: > 0, }) return;
            foreach (var skinnedMeshRenderer in bodyMeshRenderers) {
                bodyMuscle.ChangeShape(skinnedMeshRenderer, 0f);
                bodyFat.ChangeShape(skinnedMeshRenderer, 0f);
            }

            if (Animator == null && TryGetComponent(out Animator animator))
                Animator = animator;

            if (KeyAreas == null)
                KeyAreas = TryGetComponent(out AvatarKeyAreas keyAreas)
                    ? keyAreas
                    : gameObject.AddComponent<AvatarKeyAreas>();
            else
                KeyAreas.OnValidate();
        }
#endif

        void FirstSetup() {
            firstUse = false;
            vore.Setup(AllShapes, hasBallsController);
        }

        public virtual void Setup(BaseCharacter character) {
            var forceSkinUpdate = false;
            if (firstUse)
                FirstSetup();
            LoadDetails();
            var hasVagina = character.SexualOrgans.Vaginas.HaveAny();

            if (HandleDickAndBalls(character.SexualOrgans))
                forceSkinUpdate = true;
            vore.Update(character);
            foreach (var shape in AllShapes)
                SetShapes(character, hasVagina, shape);
            hairColor.SetBald(hairMeshRenderers, character.Hair.Bald);
            if (changeHairColor)
                hairColor.SetHairColor(hairMeshRenderers, character.Hair);
            SetArousal(character.SexStats.Arousal);
            UpdateBodyTypeMorphs(character.Body.Morphs);
            SetSkinTone(character.Body.SkinTone, forceSkinUpdate);
        }


        bool HandleDickAndBalls(SexualOrgans sexualOrgans) {
            var hasDick = sexualOrgans.Dicks.HaveAny();
            var hasBalls = sexualOrgans.Balls.HaveAny();
            var forceSkinUpdate = hasHideDaz
                ? hideDazDickAndBalls.Handle(bodyMeshRenderers, hasDick, hasBalls)
                : dictatorBoner.HandleHideDickAndBalls(bodyMeshRenderers, hasDick, hasDick);
            HandleDick(sexualOrgans.Dicks, hasDick);
            HandleBalls(sexualOrgans.Balls, hasBalls);
            return forceSkinUpdate;
        }

        // Athlete 10
        // Fit 17
        // Normal 21
        // obese 28
        void SetShapes(BaseCharacter character, bool hasVagina, SkinnedMeshRenderer shape) {
            var fatRatio = character.Body.GetFatRatio() - 1f;
            bodyFat.ChangeShape(shape, Mathf.Clamp(fatRatio * fatMultiplier, 0f, 120f));
            var muscleRatio = character.Body.GetMuscleRatio();
            bodyMuscle.ChangeShape(shape, Mathf.Clamp(muscleRatio * 100f, 0f, 150f));
            noBoobs.ChangeShape(shape, Mathf.Clamp((7f - character.SexualOrgans.Boobs.Biggest) * 15f, 0f, 100f));
            biggerBoobs.ChangeShape(shape, Mathf.Clamp(character.SexualOrgans.Boobs.Biggest * 2f, 0f, 300f));
            pregnant.ChangeShape(shape, PregnancyValue(character, hasVagina));
            thickness.ChangeShape(shape, character.Body.Thickset);
        }

        static float PregnancyValue(BaseCharacter character, bool hasVagina) {
            if (!hasVagina || !character.SexualOrgans.Vaginas.BaseList.Any(v => v.Womb.HasFetus)) return 0f;
            var oldestFetus = character.SexualOrgans.Vaginas.BaseList.Aggregate(0,
                (current, baseOrgan) => baseOrgan.Womb.FetusList.Select(fetus => fetus.DaysOld).Prepend(current).Max());
            return Mathf.Clamp((float)oldestFetus / PregnancyExtensions.IncubationDays * 100f, 0f, 100f);
        }

        public void SetArousal(int arousal) {
            dictatorBoner.ChangeShape(AllShapes, arousal);
            if (hasDickController)
                dickController.SetBoner(arousal);
        }

        void HandleBalls(BaseOrgansContainer balls, bool hasBalls) {
            if (hasBallsController) {
                ballsController.ShowOrHide(hasBalls);
                if (!hasBalls)
                    return;
                ballsController.SetupFluidStretch(balls);
                balls.Fluid.CurrentValueChange -= UpdateBallsStretch;
                balls.Fluid.CurrentValueChange += UpdateBallsStretch;
                ballSize = SetOrganSize(balls.Biggest);
                if (!balls.BaseList.Any(b => b.Vore.PreysIds.Any())) ballsController.ReSize(ballSize);
            } else if (dictatorBoner.hasDictatorBalls) {
                if (!hasBalls)
                    return;
                dictatorBoner.SetupFluidStretch(balls);
                balls.Fluid.CurrentValueChange -= UpdateDictatorBallsStretch;
                balls.Fluid.CurrentValueChange += UpdateDictatorBallsStretch;
                if (!balls.BaseList.Any(b => b.Vore.PreysIds.Any()))
                    dictatorBoner.SetBallsSize(SetOrganSize(balls.Biggest));
            }
        }

        void UpdateBallsStretch(float obj) => ballsController.SetFluidStretch(obj);
        void UpdateDictatorBallsStretch(float obj) => dictatorBoner.SetFluidStretch(obj);

        void HandleDick(BaseOrgansContainer sexualBaseOrgans, bool hasDick) {
            if (hasDickController) {
                DazDick(sexualBaseOrgans, hasDick);
            } else if (dictatorBoner.hasDictatorDick) {
                dictatorBoner.HideOrShowDick(hasDick);
                dictatorBoner.SetDickSize(SetOrganSize(sexualBaseOrgans.Biggest));
            }
        }

        void DazDick(BaseOrgansContainer sexualBaseOrgans, bool hasDick) {
            dickController.HideOrShow(hasDick);
            if (hasDick)
                dickController.SetDickSize(SetOrganSize(sexualBaseOrgans.Biggest));
        }

        static float SetOrganSize(float currentSize) => 0.2f + Mathf.Log(currentSize) * OrganMulti;


        public void SetSkinTone(float tone, bool forceSkinUpdate) {
            skinTone.SetSkinTone(tone, bodyMeshRenderers, forceSkinUpdate);
        }

        public void Save() {
            if (AvatarDetails.AvatarDetailsSavesDict.ContainsKey(prefab.AssetGUID))
                AvatarDetails.AvatarDetailsSavesDict[prefab.AssetGUID] =
                    new AvatarDetails.SavedValues(prefab.AssetGUID);
            else
                AvatarDetails.AvatarDetailsSavesDict.Add(prefab.AssetGUID,
                    new AvatarDetails.SavedValues(prefab.AssetGUID));
            foreach (var hairMat in hairMats)
                AvatarDetails.AvatarDetailsSavesDict[prefab.AssetGUID].MatToSave(hairMat);
        }

        void LoadDetails() {
            if (!AvatarDetails.AvatarDetailsSavesDict.TryGetValue(prefab.AssetGUID, out var saved))
                return;
            foreach (var colorSave in saved.ColorSaves) {
                if (!TryFindMaterial(colorSave, out var firstOrDefault)) continue;
                if (ColorUtility.TryParseHtmlString($"#{colorSave.ColorName}", out var savedColor))
                    firstOrDefault.color = savedColor;
            }

            bool TryFindMaterial(AvatarDetails.ColorSave colorSave, out Material firstOrDefault) {
                foreach (var m in hairMats) {
                    if (m.name != colorSave.MatName)
                        continue;
                    firstOrDefault = m;
                    return true;
                }

                firstOrDefault = default;
                return false;
            }
        }

        [Serializable]
        public struct BlendShape {
            [SerializeField] bool hasShape;
            [SerializeField] int[] ids;
            [SerializeField, Range(-50, 50),] float offSet;
            public void ChangeShape(SkinnedMeshRenderer shape, float value) {
                if (!hasShape || ids.Length == 0)
                    return;
                foreach (var id in ids)
                    if (id < shape.sharedMesh.blendShapeCount)
                        shape.SetBlendShapeWeight(id, value + offSet);
            }
#if UNITY_EDITOR
            public void EditorQuickAdd(int id) {
                if (ids.Contains(id)) return;
                ids = new List<int>(ids) { id, }.ToArray();
                hasShape = true;
            }
#endif
        }
#if UNITY_EDITOR
        [ContextMenu("Get hair renders")]
        void GetSkinnedMeshRenderers() {
            bodyMeshRenderers ??= new List<SkinnedMeshRenderer>();
            hairMeshRenderers ??= new List<SkinnedMeshRenderer>();
            foreach (var meshRenderer in GetComponentsInChildren<SkinnedMeshRenderer>())
                if (meshRenderer.name.ToLower().Contains("hair")) {
                    if (hairMeshRenderers.Contains(meshRenderer) == false)
                        hairMeshRenderers.Add(meshRenderer);
                } else if (bodyMeshRenderers.Contains(meshRenderer) == false) {
                    bodyMeshRenderers.Add(meshRenderer);
                }
        }

        [ContextMenu("Get ids")]
        void GetMorphIds() {
            if (AllShapes == null || !AllShapes.Any())
                return;
            var meshRenderer = AllShapes.First();
            var counts = meshRenderer.sharedMesh.blendShapeCount;
            for (var i = 0; i < counts; i++) {
                var shapeName = meshRenderer.sharedMesh.GetBlendShapeName(i).ToLower();
                if (shapeName.Contains("hd details")) {
                    print($"Skipped {shapeName} ({i})");
                } else if (vore.EditorQuickSetup(shapeName, i)) { } else if (shapeName.Contains("breasts")) {
                    if (shapeName.Contains("gone"))
                        noBoobs.EditorQuickAdd(i);
                    else if (shapeName.Contains("size"))
                        biggerBoobs.EditorQuickAdd(i);
                } else if (shapeName.Contains("heavy")) {
                    bodyFat.EditorQuickAdd(i);
                } else if (shapeName.Contains("fbmbody")) {
                    bodyMuscle.EditorQuickAdd(i);
                } else if (shapeName.Contains("fbmthin")) {
                    thickness.EditorQuickAddThin(i);
                } else if (shapeName.Contains("fbmvoluptuous") || shapeName.Contains("fbmstocky")) {
                    thickness.EditorQuickAddThick(i);
                } else if (shapeName.Contains("preg")) {
                    pregnant.EditorQuickAdd(i);
                } else if (bodyShapes.EditorQuickSetup(shapeName, i)) { } else {
                    print($"Unassigned {shapeName} ({i})");
                }
            }
        }


#endif
    }
}