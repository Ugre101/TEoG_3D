using UnityEngine;

namespace Character.Organs.Fluids.UI
{
    public class FluidInfos : MonoBehaviour
    {
        [SerializeField] FluidInfo cumInfo;
        [SerializeField] FluidInfo milkInfo;

        BaseCharacter character;
        public void Setup(BaseCharacter myCharacter)
        {
            character = myCharacter;
            UpdateCum(0f);
            UpdateMilk(0f);
            character.SexualOrgans.Balls.Fluid.CurrentValueChange += UpdateCum;
            character.SexualOrgans.Boobs.Fluid.CurrentValueChange += UpdateMilk;
        }

        void OnDisable()
        {
            if (character == null)
                return;
            character.SexualOrgans.Balls.Fluid.CurrentValueChange -= UpdateCum;
            character.SexualOrgans.Boobs.Fluid.CurrentValueChange -= UpdateMilk;
        }

        void OnDestroy()
        {
            if (character == null)
                return;
            character.SexualOrgans.Balls.Fluid.CurrentValueChange -= UpdateCum;
            character.SexualOrgans.Boobs.Fluid.CurrentValueChange -= UpdateMilk;
        }

        void UpdateMilk(float obj) => milkInfo.UpdateFluid(character.SexualOrgans.Boobs,character.Body.Height.Value);
        void UpdateCum(float obj) => cumInfo.UpdateFluid(character.SexualOrgans.Balls,character.Body.Height.Value);
    }
}