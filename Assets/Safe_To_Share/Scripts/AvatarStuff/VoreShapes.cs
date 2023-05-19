using System;
using System.Collections.Generic;
using System.Linq;
using Character;
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
        [SerializeField] CharacterAvatar.BlendShape cockVore;
        float ballsStretch;
        bool ballsVore;

        VoreStruggle breastVoreStruggle;

        bool hasBallsController;
        VoreStruggle oralVoreStruggle;
        SkinnedMeshRenderer[] shapes;
        VoreStruggle unbirthStruggle;
        VoreStruggle cockVoreStruggle;
#if UNITY_EDITOR
        public bool EditorQuickSetup(string shapeName, int id)
        {
            if (shapeName.Contains("unbirth"))
            {
                unBirth.EditorQuickAdd(id);
                return true;
            }

            if (!shapeName.Contains("vore")) return false;
            if (shapeName.Contains("breast"))
            {
                breastVore.EditorQuickAdd(id);
                return true;
            }

            if (shapeName.Contains("cock"))
            {
                cockVore.EditorQuickAdd(id);
                return true;
            }

            voreBelly.EditorQuickAdd(id);
            return true;

        }
#endif

        public void Setup(IEnumerable<SkinnedMeshRenderer> skinnedMeshRenderers, bool haveBallsController)
        {
            shapes = skinnedMeshRenderers.ToArray();
            hasBallsController = haveBallsController;
            breastVoreStruggle = new VoreStruggle(breastVore);
            oralVoreStruggle = new VoreStruggle(voreBelly);
            unbirthStruggle = new VoreStruggle(unBirth);
            cockVoreStruggle = new VoreStruggle(cockVore);
        }

        public void Update(BaseCharacter character)
        {
            HandleOralVore(character);
            unbirthStruggle.HandleOrgan(shapes,character,character.SexualOrgans.Vaginas,4f);
            breastVoreStruggle.HandleOrgan(shapes,character,character.SexualOrgans.Boobs,4f);
            cockVoreStruggle.HandleOrgan(shapes,character,character.SexualOrgans.Balls,4f);
            
            var ballsList = character.SexualOrgans.Balls.BaseList.ToArray();
            ballsVore = ballsList.Any(l => l.Vore.PreysIds.Count > 0);
            ballsStretch = ballsVore ? ballsList.Max(b => b.Vore.Stretch) : 0f;
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
            return Mathf.Clamp(stomachWeight / containerSize, 0f, 200f);
        }

        public void TickVoreStruggle(DazBallsController ballsController)
        {
            if (!OptionalContent.Vore.Enabled)
                return;
            foreach (var shape in shapes)
            {
                oralVoreStruggle.Tick(shape);
                unbirthStruggle.Tick(shape);
                breastVoreStruggle.Tick(shape);
                cockVoreStruggle.Tick(shape);
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

            public void HandleOrgan(SkinnedMeshRenderer[] shapes, BaseCharacter character, BaseOrgansContainer container,float divValue)
            {
                float avatarStretch = GetSexualOrganStretch(character, container, divValue);
                Setup(shapes, container.HasPrey(), avatarStretch);
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