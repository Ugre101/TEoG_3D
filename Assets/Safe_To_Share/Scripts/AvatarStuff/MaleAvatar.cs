using Character;
using Character.GenderStuff;
using UnityEngine;

namespace AvatarStuff
{
    public sealed class MaleAvatar : CharacterAvatar
    {
        [SerializeField] BlendShape turnFemale;

        public override void Setup(BaseCharacter character)
        {
            base.Setup(character);
            float femaleValue = GenderSettings.MaleTurningFemaleValue(character);
            foreach (var meshRenderer in bodyMeshRenderers) 
                turnFemale.ChangeShape(meshRenderer,femaleValue);
        }
    }
}