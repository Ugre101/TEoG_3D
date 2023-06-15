using Character;
using Character.GenderStuff;
using UnityEngine;

namespace AvatarStuff {
    public sealed class FemaleAvatar : CharacterAvatar {
        [SerializeField] BlendShape turnMale;

        public override void Setup(BaseCharacter character) {
            base.Setup(character);
            var maleValue = GenderSettings.FemaleTurningMaleValue(character);
            foreach (var meshRenderer in bodyMeshRenderers)
                turnMale.ChangeShape(meshRenderer, maleValue);
        }
    }
}