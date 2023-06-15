using System.Collections.Generic;
using System.Linq;
using Character;
using UnityEngine;

namespace AvatarStuff {
    [CreateAssetMenu(fileName = "Avatar Dictionary", menuName = "Character/Avatar/Dictionary", order = 0)]
    public sealed class AvatarDict : ScriptableObject {
        [SerializeField] CharacterAvatar defaultAvatar;
        [SerializeField] List<CharacterAvatar> avatars = new();

        public CharacterAvatar GetAvatar(BaseCharacter character) =>
            avatars.Count == 0 ? defaultAvatar : BestMatch(character);

        CharacterAvatar BestMatch(BaseCharacter character) {
#if UNITY_EDITOR
            if (sfwMode && sfwAvatar != null)
                return sfwAvatar;
#endif
            CharacterAvatar bestMatch = null;
            foreach (var avatar in avatars) {
                if (!avatar.SupportedRaces.Contains(character.RaceSystem.Race))
                    continue;
                if (avatar.SupportedGenders.Contains(character.Gender))
                    return avatar;
                if (bestMatch == null)
                    bestMatch = avatar;
            }

            return bestMatch ? bestMatch : defaultAvatar;
        }
#if UNITY_EDITOR
        [SerializeField] CharacterAvatar sfwAvatar;
        [SerializeField] bool sfwMode;
#endif
    }
}