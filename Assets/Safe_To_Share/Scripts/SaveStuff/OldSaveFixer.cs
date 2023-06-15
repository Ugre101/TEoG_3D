using System.Collections.Generic;
using System.Linq;
using Character;
using Character.EssenceStuff;
using Character.PlayerStuff;
using Character.VoreStuff;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace SaveStuff {
    public sealed class OldSaveFixer : MonoBehaviour {
        [SerializeField] List<AssetReference> standardAbilities = new();
        public static OldSaveFixer Instance { get; private set; }

        void Awake() {
            if (Instance == null)
                Instance = this;
            else
                Destroy(gameObject);
        }

        public void FixPlayer(Player player) {
            CheckAbilities(player);
            CheckVore(player);
            CheckButt(player);
        }

        static void CheckButt(BaseCharacter player) {
            if (player.SexualOrgans.Anals.HaveAny())
                return;
            Essence tempEss = new();
            tempEss.GainEssence(9999);
            player.SexualOrgans.Anals.TryGrowNew(tempEss);
            Debug.Log("Added anal organ");
        }

        void CheckAbilities(ControlledCharacter player) {
            foreach (var standardAbility in standardAbilities) {
                if (player.AndSpellBook.Abilities.Contains(standardAbility.AssetGUID)) continue;
                player.AndSpellBook.LearnAbility(standardAbility.AssetGUID);
                Debug.LogWarning("Didn't have " + standardAbility.SubObjectName);
            }
        }


        static void CheckVore(BaseCharacter player) {
            var faults = player.Vore.Stomach.PreysIds.RemoveAll(id => !VoredCharacters.PreyDict.ContainsKey(id)) +
                         player.SexualOrgans.Containers.Values.Sum(organCon => organCon.BaseList.Sum(organ =>
                             organ.Vore.PreysIds.RemoveAll(preyId => !VoredCharacters.PreyDict.ContainsKey(preyId))));
            if (faults > 0)
                Debug.LogWarning($"{faults} preys couldn't be found");
        }
    }
}