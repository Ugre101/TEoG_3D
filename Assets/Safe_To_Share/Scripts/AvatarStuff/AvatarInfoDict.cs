using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Character;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace AvatarStuff
{
    [CreateAssetMenu(fileName = "Avatar info Dictionary", menuName = "Character/Avatar/Info Dictionary", order = 0)]
    public sealed class AvatarInfoDict : ScriptableObject
    {
        [SerializeField] AvatarInfo defaultAvatar;
        [SerializeField] List<AvatarInfo> avatars = new();

        public AssetReference GetAvatar(BaseCharacter character, bool player)
        {
            if (avatars.Count == 0)
                return player ? defaultAvatar.PlayerPrefab : defaultAvatar.Prefab;
            return BestMatch(character, player);
        }

        public async Task<GameObject> GetAvatarLoaded(BaseCharacter character, bool player) => avatars.Count == 0
            ? await defaultAvatar.GetLoadedPrefab(player)
            : await BestMatchGame(character, player);

        AssetReference BestMatch(BaseCharacter character, bool player)
        {
#if UNITY_EDITOR
            if (sfwMode && sfwAvatar != null)
            {
            }
#endif
            return player ? GetInfo(character).PlayerPrefab : GetInfo(character).Prefab;
        }

        AvatarInfo lastPlayer;
        public AvatarInfo GetInfo(BaseCharacter character)
        {
#if UNITY_EDITOR
            if (sfwMode && sfwAvatar is null)
            {
            }
#endif
            AvatarInfo bestMatch = null;
            foreach (var avatar in avatars)
            {
                if (!avatar.SupportedRaces.Contains(character.RaceSystem.Race)) continue;
                if (avatar.SupportedGenders.Contains(character.Gender)) return avatar;
                bestMatch ??= avatar;
            }

            return bestMatch ? bestMatch : defaultAvatar;
        }

        async Task<GameObject> BestMatchGame(BaseCharacter character, bool player)
        {
#if UNITY_EDITOR
            if (sfwMode && sfwAvatar is null)
            {
            }
#endif
            var avatarInfo = GetInfo(character);
            return await avatarInfo.GetLoadedPrefab(player);
        }
#if UNITY_EDITOR
        [SerializeField] AvatarInfo sfwAvatar;
        [SerializeField] bool sfwMode;
#endif
    }
}