using System;
using System.Linq;
using System.Threading.Tasks;
using Character.EssenceStuff;
using Character.LevelStuff;
using Character.Race.Races;
using Character.VoreStuff;
using CustomClasses;
using Items;
using Safe_To_Share.Scripts.CustomClasses;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Character.CreateCharacterStuff
{
    [CreateAssetMenu(menuName = "Character/Presets/Create CharacterPreset", fileName = "CharacterPreset")]
    public class CharacterPreset : ScriptableObject
    {
        [SerializeField] StartGender startGender;
        [SerializeField] StartIdentity startIdentity;
        [SerializeField] StartStats startStats;
        [SerializeField] DropSerializableObject<SerializableScriptableObject>[] startAbilitiesGuids;
        [SerializeField] AddAmountOf<Item>[] startItemsWithAmountOf;
        [SerializeField] BasicPerk[] startPerks;
        [SerializeField] EssencePerk[] startEssencePerks;
        [SerializeField] VorePerk[] startVorePerks;
        [SerializeField] AssetReference startRaceRef;
        [SerializeField] StartBody startBody;
        [SerializeField] StartHair startHair;
        [SerializeField] StartSkinColor startSkinColor;

        bool loading, done;
        AsyncOperationHandle<BasicRace> raceOp;
        BasicRace startRace;

        //private IEnumerable<AsyncOperationHandle<Ability>> ai;
        public void DefaultValues() => startBody.Default();

        public async Task LoadAssets()
        {
            if (startRace != null)
                return;
            if (loading)
            {
                while (!done) await Task.Delay(100);
                return;
            }

            loading = true;
            raceOp = startRaceRef.LoadAssetAsync<BasicRace>();
            await raceOp.Task;
            if (raceOp.Status == AsyncOperationStatus.Succeeded)
            {
                startRace = raceOp.Result;
                done = true;
                Addressables.Release(raceOp);
            }
        }


        public void UnLoad()
        {
            startRace = null;
            loading = false;
            done = false;
        }

        public CreateCharacter NewCharacter()
        {
            var longWay = Array.Empty<string>();
            if (startAbilitiesGuids is { Length: > 0, })
                longWay = startAbilitiesGuids.Select(dropSerializableObject => dropSerializableObject.guid).ToArray();
            return new CreateCharacter(startIdentity, startStats.GetStats(),
                longWay, startItemsWithAmountOf, startRace, startGender, startBody, startPerks, startHair, startSkinColor);
        }
    }
}