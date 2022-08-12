using System;
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

namespace AvatarStuff
{
    [RequireComponent(typeof(FootSteps))]
    public partial class CharacterAvatar : MonoBehaviour
    {
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

        float ballSize;
        bool firstUse = true;
        bool forceSkinUpdate;
        float lastTick;
        SkinnedMeshRenderer[] shapes;

        public IEnumerable<SkinnedMeshRenderer> AllShapes
        {
            get
            {
                if (shapes == null)
                {
                    var temp = bodyMeshRenderers.Concat(hairMeshRenderers);
                    temp = temp.Concat(detailsMeshRenderers);
                    shapes = temp.ToArray();
                }

                return shapes;
            }
        }

        public IEnumerable<Gender> SupportedGenders => supportedGenders;
        public IEnumerable<BasicRace> SupportedRaces => supportedRaces;

        public AssetReference Prefab => prefab;

        public Material[] HairMats => hairMats;

        void Update()
        {
            if (firstUse || Time.time < lastTick + 1f)
                return;
            lastTick = Time.time;
            vore.TickVoreStruggle(ballsController);
        }


#if UNITY_EDITOR
        void OnValidate()
        {
            hasDickController = dickController != null;
            hasBallsController = ballsController != null;
            hasHideDaz = hideDazDickAndBalls != null;
            if (Application.isPlaying) return;
            if (bodyMeshRenderers is not { Count: > 0, }) return;
            foreach (SkinnedMeshRenderer skinnedMeshRenderer in bodyMeshRenderers)
            {
                bodyMuscle.ChangeShape(skinnedMeshRenderer, 0f);
                bodyFat.ChangeShape(skinnedMeshRenderer, 0f);
            }
        }
#endif

        void FirstSetup()
        {
            firstUse = false;
            vore.Setup(AllShapes, hasBallsController);
        }

        public virtual void Setup(BaseCharacter character)
        {
            if (firstUse)
                FirstSetup();
            LoadDetails();
            bool hasVagina = character.SexualOrgans.Vaginas.HaveAny();
            HandleDickAndBalls(character.SexualOrgans);
            vore.Update(character);
            foreach (SkinnedMeshRenderer shape in AllShapes)
                SetShapes(character, hasVagina, shape);
            hairColor.SetBald(hairMeshRenderers, character.Hair.Bald);
            if (changeHairColor)
                hairColor.SetHairColor(hairMeshRenderers, character.Hair);
            SetArousal(character.SexStats.Arousal);
            UpdateBodyTypeMorphs(character.Body.Morphs);
            SetSkinTone(character.Body.SkinTone);
            forceSkinUpdate = false;
        }

        void HandleDickAndBalls(SexualOrgans sexualOrgans)
        {
            bool hasDick = sexualOrgans.Dicks.HaveAny();
            bool hasBalls = sexualOrgans.Balls.HaveAny();
            forceSkinUpdate = hasHideDaz ? hideDazDickAndBalls.Handle(bodyMeshRenderers, hasDick, hasBalls) : dictatorBoner.Handle(bodyMeshRenderers,hasDick,hasDick);
            HandleDick(sexualOrgans.Dicks, hasDick);
            HandleBalls(sexualOrgans.Balls, hasBalls);
        }

        // Athlete 10
        // Fit 17
        // Normal 21
        // obese 28
        void SetShapes(BaseCharacter character, bool hasVagina, SkinnedMeshRenderer shape)
        {
            float fatRatio = character.Body.GetFatRatio() - 1f;
            bodyFat.ChangeShape(shape, Mathf.Clamp(fatRatio * fatMultiplier, 0f, 120f));
            float muscleRatio = character.Body.GetMuscleRatio();
            bodyMuscle.ChangeShape(shape, Mathf.Clamp(muscleRatio * 100f, 0f, 150f));
            noBoobs.ChangeShape(shape, Mathf.Clamp((7f - character.SexualOrgans.Boobs.Biggest) * 15f, 0f, 100f));
            biggerBoobs.ChangeShape(shape, Mathf.Clamp(character.SexualOrgans.Boobs.Biggest * 2f, 0f, 300f));
            pregnant.ChangeShape(shape, PregnancyValue(character, hasVagina));
            thickness.ChangeShape(shape, character.Body.Thickset);
        }

        static float PregnancyValue(BaseCharacter character, bool hasVagina)
        {
            if (!hasVagina || !character.SexualOrgans.Vaginas.List.Any(v => v.Womb.HasFetus)) return 0f;
            int oldestFetus = character.SexualOrgans.Vaginas.List.Aggregate(0,
                (current, baseOrgan) => baseOrgan.Womb.FetusList.Select(fetus => fetus.DaysOld).Prepend(current).Max());
            return Mathf.Clamp((float)oldestFetus / PregnancyExtensions.IncubationDays * 100f, 0f, 100f);
        }

        public void SetArousal(int arousal)
        {
            dictatorBoner.ChangeShape(AllShapes, arousal);
            if (hasDickController)
                dickController.SetBoner(arousal);
        }

        void HandleBalls(OrgansContainer balls, bool hasBalls)
        {
            if (!hasBallsController)
                return;
            ballsController.ShowOrHide(hasBalls);
            if (!hasBalls)
                return;
            ballsController.SetupFluidStretch(balls);
            balls.Fluid.CurrentValueChange -= UpdateBallsStretch;
            balls.Fluid.CurrentValueChange += UpdateBallsStretch;
            ballSize = SetOrganSize(balls.Biggest);
            if (!balls.List.Any(b => b.Vore.PreysIds.Any())) ballsController.ReSize(ballSize);
        }

        void UpdateBallsStretch(float obj) => ballsController.SetFluidStretch(obj);

        void HandleDick(OrgansContainer sexualOrgans, bool hasDick)
        {
            if (hasDickController)
                DazDick(sexualOrgans, hasDick);
            else
            {
                dictatorBoner.HideOrShow(hasDick, bodyMeshRenderers);
                dictatorBoner.SetDickSize(SetOrganSize(sexualOrgans.Biggest));
            }
        }

        void DazDick(OrgansContainer sexualOrgans, bool hasDick)
        {
            dickController.HideOrShow(hasDick);
            if (hasDick)
                dickController.SetDickSize(SetOrganSize(sexualOrgans.Biggest));
        }

        static float SetOrganSize(float currentSize) => 0.2f + Mathf.Log(currentSize) * OrganMulti;


        public void SetSkinTone(float tone)
        {
            skinTone.SetSkinTone(tone, bodyMeshRenderers, forceSkinUpdate);
            forceSkinUpdate = false;
        }

        public void Save()
        {
            if (AvatarDetails.AvatarDetailsSavesDict.ContainsKey(prefab.AssetGUID))
                AvatarDetails.AvatarDetailsSavesDict[prefab.AssetGUID] =
                    new AvatarDetails.SavedValues(prefab.AssetGUID);
            else
                AvatarDetails.AvatarDetailsSavesDict.Add(prefab.AssetGUID,
                    new AvatarDetails.SavedValues(prefab.AssetGUID));
            foreach (Material hairMat in hairMats)
                AvatarDetails.AvatarDetailsSavesDict[prefab.AssetGUID].MatToSave(hairMat);
        }

        void LoadDetails()
        {
            if (!AvatarDetails.AvatarDetailsSavesDict.TryGetValue(prefab.AssetGUID, out var saved))
                return;
            foreach (AvatarDetails.ColorSave colorSave in saved.colorSaves)
            {
                Material firstOrDefault = hairMats.FirstOrDefault(m => m.name == colorSave.matName);
                if (firstOrDefault != null &&
                    ColorUtility.TryParseHtmlString($"#{colorSave.colorName}", out Color savedColor))
                    firstOrDefault.color = savedColor;
            }
        }

        [Serializable]
        public struct BlendShape
        {
            [SerializeField] bool hasShape;
            [SerializeField] int[] ids;
            [SerializeField, Range(-50, 50),] float offSet;
            public void ChangeShape(SkinnedMeshRenderer shape, float value)
            {
                if (!hasShape || ids.Length == 0)
                    return;
                foreach (int id in ids)
                    if (id < shape.sharedMesh.blendShapeCount)
                        shape.SetBlendShapeWeight(id, value + offSet);
            }
#if UNITY_EDITOR
            public void EditorQuickAdd(int id)
            {
                if (ids.Contains(id)) return;
                ids = new List<int>(ids) { id, }.ToArray();
                hasShape = true;
            }
#endif
        }
#if UNITY_EDITOR
        [ContextMenu("Get hair renders")]
        void GetSkinnedMeshRenderers()
        {
            bodyMeshRenderers ??= new List<SkinnedMeshRenderer>();
            hairMeshRenderers ??= new List<SkinnedMeshRenderer>();
            foreach (var meshRenderer in GetComponentsInChildren<SkinnedMeshRenderer>())
                if (meshRenderer.name.ToLower().Contains("hair"))
                {
                    if (hairMeshRenderers.Contains(meshRenderer) == false)
                        hairMeshRenderers.Add(meshRenderer);
                }
                else if (bodyMeshRenderers.Contains(meshRenderer) == false)
                    bodyMeshRenderers.Add(meshRenderer);
        }

        [ContextMenu("Get ids")]
        void GetMorphIds()
        {
            if (AllShapes == null || !AllShapes.Any())
                return;
            var meshRenderer = AllShapes.First();
            int counts = meshRenderer.sharedMesh.blendShapeCount;
            for (int i = 0; i < counts; i++)
            {
                string shapeName = meshRenderer.sharedMesh.GetBlendShapeName(i).ToLower();
                if (shapeName.Contains("hd details"))
                    print($"Skipped {shapeName} ({i})");
                else if (vore.EditorQuickSetup(shapeName, i))
                {
                }
                else if (shapeName.Contains("breasts"))
                {
                    if (shapeName.Contains("gone"))
                        noBoobs.EditorQuickAdd(i);
                    else if (shapeName.Contains("size"))
                        biggerBoobs.EditorQuickAdd(i);
                }
                else if (shapeName.Contains("heavy"))
                    bodyFat.EditorQuickAdd(i);
                else if (shapeName.Contains("fbmbody"))
                    bodyMuscle.EditorQuickAdd(i);
                else if (shapeName.Contains("fbmthin"))
                    thickness.EditorQuickAddThin(i);
                else if (shapeName.Contains("fbmvoluptuous") || shapeName.Contains("fbmstocky"))
                    thickness.EditorQuickAddThick(i);
                else if (shapeName.Contains("preg"))
                    pregnant.EditorQuickAdd(i);
                else if (bodyShapes.EditorQuickSetup(shapeName, i))
                {
                }
                else
                    print($"Unassigned {shapeName} ({i})");
            }
        }


#endif
    }
}