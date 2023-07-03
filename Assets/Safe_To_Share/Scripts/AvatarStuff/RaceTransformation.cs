using Character;
using Character.Race.Races;
using UnityEngine;

namespace Safe_To_Share.Scripts.AvatarStuff {
    public class RaceTransformation : MonoBehaviour {
        [SerializeField] BasicRace orc;
        [SerializeField] BasicRace bovine;
        const string OrcProp = "_Turn_Orc";
        const string BovineProp = "_Turn_Bovine";
        static readonly int TurnOrc = Shader.PropertyToID(OrcProp);
        static readonly int TurnBovine = Shader.PropertyToID(BovineProp);

        public void Check(Renderer renderer, BaseCharacter character) {
            float essTot = 0;
            float orcEss = 0, bovineEss = 0;
            foreach (var raceEssence in character.RaceSystem.AllRaceEssence) {
                if (raceEssence.Race.Guid == orc.Guid) {
                    orcEss += raceEssence.Amount;
                }else if (raceEssence.Race.Guid == bovine.Guid) {
                    bovineEss += raceEssence.Amount;
                }
                essTot += raceEssence.Amount;
            }

            float orcPer = orcEss / essTot;
            float bovinePer = bovineEss / essTot;

            print($"orc {orcPer}");
            print($"Bovine {bovinePer}");

            foreach (var skin in renderer.materials) {
                if (skin.HasProperty("_Turn_Orc")) {
                    skin.SetFloat(TurnOrc,orcPer);
                }
                if (skin.HasProperty(BovineProp)) {
                    skin.SetFloat(TurnBovine, bovinePer);
                }
            }
            
        }
    }
}