using System;
using System.Collections.Generic;
using System.Linq;
using Character;
using Character.Organs;
using Character.Organs.OrgansContainers;
using Character.VoreStuff;
using Safe_To_Share.Scripts.Static;
using UnityEngine;
using Random = UnityEngine.Random;

namespace AvatarStuff
{
    [Serializable]
    public class VoreShapes
    {
        [SerializeField] CharacterAvatar.BlendShape voreBelly;
        [SerializeField] CharacterAvatar.BlendShape unBirth;
        [SerializeField] CharacterAvatar.BlendShape breastVore;
        float ballsStretch;
        bool ballsVore;

        VoreStruggle breastVoreStruggle;

        bool hasBallsController;
        VoreStruggle oralVoreStruggle;
        SkinnedMeshRenderer[] shapes;
        VoreStruggle unbirthStruggle;
#if UNITY_EDITOR
        public bool EditorQuickSetup(string shapeName, int id)
        {
            if (shapeName.Contains("unbirth"))
            {
                unBirth.EditorQuickAdd(id);
                return true;
            }

            if (shapeName.Contains("vore"))
            {
                if (shapeName.Contains("breast"))
                {
                    breastVore.EditorQuickAdd(id);
                    return true;
                }

                voreBelly.EditorQuickAdd(id);
                return true;
            }

            return false;
        }
#endif

        public void Setup(IEnumerable<SkinnedMeshRenderer> skinnedMeshRenderers, bool haveBallsController)
        {
            shapes = skinnedMeshRenderers.ToArray();
            hasBallsController = haveBallsController;
            breastVoreStruggle = new VoreStruggle(breastVore);
            oralVoreStruggle = new VoreStruggle(voreBelly);
            unbirthStruggle = new VoreStruggle(unBirth);
        }

        public void Update(BaseCharacter character)
        {
            HandleOralVore(character);
            HandleUnBirth(character);
            HandleBreastVore(character);
            var ballsList = character.SexualOrgans.Balls.BaseList.ToArray();
            ballsVore = ballsList.Any(l => l.Vore.PreysIds.Count > 0);
            ballsStretch = ballsVore ? ballsList.Max(b => b.Vore.Stretch) : 0f;
        }

        void HandleBreastVore(BaseCharacter character)
        {
            var boobsList = character.SexualOrgans.Boobs.BaseList.ToArray();
            bool breastVored = boobsList.Any(b => b.Vore.PreysIds.Any());
            float avatarStretch = 0f;
            if (character.SexualOrgans.Boobs.HaveAny())
            {
                List<int> preyIds = new();
                foreach (BaseOrgan baseOrgan in boobsList)
                    preyIds.AddRange(baseOrgan.Vore.PreysIds);
                avatarStretch = AvatarStretch(character.Body.Height.Value / 4f, preyIds);
            }

            breastVoreStruggle.Setup(shapes, breastVored, avatarStretch);
        }

        void HandleUnBirth(BaseCharacter character)
        {
            var vaginasList = character.SexualOrgans.Vaginas.BaseList.ToArray();
            bool unBirthVored = vaginasList.Any(v => v.Vore.PreysIds.Any());
            float avatarStretch = 0;
            avatarStretch = GetSexualOrganStretch(character, character.SexualOrgans.Vaginas, 4f);

            // unBirthVored ? vaginasList.Max(v => v.Vore.Stretch) : 0
            unbirthStruggle.Setup(shapes, unBirthVored, avatarStretch);
        }

        static float GetSexualOrganStretch(BaseCharacter character, BaseOrgansContainer baseOrgansContainer, float divValue)
        {
            var avatarStretch = 0f;
            if (!baseOrgansContainer.HaveAny()) return avatarStretch;
            List<int> preyIds = new();
            foreach (var baseOrgan in baseOrgansContainer.BaseList)
                preyIds.AddRange(baseOrgan.Vore.PreysIds);
            avatarStretch = AvatarStretch(character.Body.Height.Value / divValue, preyIds);

            return avatarStretch;
        }

        void HandleOralVore(BaseCharacter character)
        {
            bool stomachVore = character.Vore.Stomach.PreysIds.Any();
            float avatarStretch = AvatarStretch(character.Body.Height.Value / 3f, character.Vore.Stomach.PreysIds);
            oralVoreStruggle.Setup(shapes, stomachVore, avatarStretch);
        }

        static float AvatarStretch(float containerSize, IEnumerable<int> preyIds)
        {
            float stomachWeight = VoredCharacters.CurrentPreyTotalWeight(preyIds);
            float avatarStretch = Mathf.Clamp(stomachWeight / containerSize, 0f, 200f);
            return avatarStretch;
        }

        public void TickVoreStruggle(DazBallsController ballsController)
        {
            if (!OptionalContent.Vore.Enabled)
                return;
            foreach (SkinnedMeshRenderer shape in shapes)
            {
                oralVoreStruggle.Tick(shape);
                unbirthStruggle.Tick(shape);
                breastVoreStruggle.Tick(shape);
                if (!hasBallsController || !ballsVore) continue;
                float newSize = ballsController.currentSize + ballsController.currentSize / 2 *
                    Mathf.Max(0, ballsStretch + Random.Range(-0.05f, 0.05f));
                ballsController.ReSize(newSize);
            }
        }

        class VoreStruggle
        {
            bool active;
            float stretch;
            CharacterAvatar.BlendShape voreShape;

            public VoreStruggle(CharacterAvatar.BlendShape voreShape) => this.voreShape = voreShape;

            float Value => Mathf.Clamp(stretch + Random.Range(-3f, 3f), 0f, 200f);
            void SetStretch(float value) => stretch = value * 100f;

            public void Tick(SkinnedMeshRenderer shape)
            {
                if (active)
                    voreShape.ChangeShape(shape, Value);
            }

            public void Setup(SkinnedMeshRenderer[] shapes, bool isActive, float voreStrecht)
            {
                active = isActive;
                if (active)
                {
                    SetStretch(voreStrecht);
                    return;
                }

                foreach (var shape in shapes)
                    voreShape.ChangeShape(shape, 0);
            }
        }
    }
}