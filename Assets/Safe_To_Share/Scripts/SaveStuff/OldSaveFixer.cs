using System.Collections.Generic;
using System.Linq;
using Character;
using Character.PlayerStuff;
using Character.VoreStuff;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace SaveStuff
{
    public class OldSaveFixer : MonoBehaviour
    {
        [SerializeField] List<AssetReference> standardAbilities = new();
        public static OldSaveFixer Instance { get; private set; }

        void Awake()
        {
            if (Instance == null)
                Instance = this;
            else
                Destroy(gameObject);
        }

        public void FixPlayer(Player player)
        {
            CheckAbilities(player);
            CheckVore(player);
        }

        void CheckAbilities(ControlledCharacter player)
        {
            foreach (var standardAbility in standardAbilities)
                if (!player.AndSpellBook.Abilities.Contains(standardAbility.AssetGUID))
                {
                    player.AndSpellBook.LearnAbility(standardAbility.AssetGUID);
                    Debug.LogWarning("Didn't have " + standardAbility.SubObjectName);
                }
        }


        static void CheckVore(BaseCharacter player)
        {
            int faults = player.Vore.Stomach.PreysIds.RemoveAll(id => !VoredCharacters.PreyDict.ContainsKey(id)) +
                         player.SexualOrgans.Containers.Values.Sum(organCon => organCon.List.Sum(organ =>
                             organ.Vore.PreysIds.RemoveAll(preyId => !VoredCharacters.PreyDict.ContainsKey(preyId))));
            if (faults > 0)
                Debug.LogWarning($"{faults} preys couldn't be found");
        }
    }
}