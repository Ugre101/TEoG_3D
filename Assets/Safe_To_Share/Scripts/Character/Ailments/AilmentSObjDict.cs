using System.Collections.Generic;
using Character.BodyStuff;
using UnityEngine;

namespace Character.Ailments
{
    [CreateAssetMenu(menuName = "Create AilmentSObjDict", fileName = "AilmentSObjDict", order = 0)]
    public sealed class AilmentSObjDict : ScriptableObject
    {
        [SerializeField] List<AilmentSObj> hunger = new List<AilmentSObj>();
        [SerializeField] List<AilmentSObj> piss = new List<AilmentSObj>();

        void OnValidate()
        {
            hunger.Sort();
            piss.Sort();
        }


        public void CheckAilments(BaseCharacter character)
        {
            var hungerValue = 1f -  character.Body.GetFatRatio();
            Debug.Log($"Hunger value {hungerValue}");
            GainAndCure(character, hungerValue, hunger);
            GainAndCure(character,character.BodyFunctions.Bladder.Pressure(),piss);
        }

        void GainAndCure(BaseCharacter character, float value , List<AilmentSObj> list)
        {
            bool match = false;
            foreach (var obj in list)
            {
                if (match is false && obj.ThreesHold < value)
                {
                    obj.Gain(character);
                    match = true;
                }
                else
                    obj.Cure(character);
            }
        }
    }
}