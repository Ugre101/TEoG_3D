using AvatarStuff;
using Character;
using UnityEngine;
using UnityEngine.UI;

namespace Safe_To_Share.Scripts.Holders.UI
{
    public sealed class BodyMorphPanel : MonoBehaviour
    {
        [SerializeField] BodyMorphSliders sliders;
        [Header("Sorting"), SerializeField,] Button all;
        [SerializeField] Button body, face;
        PlayerHolder playerHolder;
        CharacterAvatar Current => playerHolder.Changer.CurrentAvatar;

        void Start()
        {
            all.onClick.AddListener(Setup);
            body.onClick.AddListener(SortBody);
            face.onClick.AddListener(SortFace);
        }

        public void Enter(PlayerHolder component)
        {
            playerHolder = component;
            gameObject.SetActive(true);
            Setup();
        }

        BodyMorphs.AvatarBodyMorphs GetMatch()
        {
            var assetGuid = Current.Prefab.AssetGUID;
            if (!playerHolder.Player.Body.Morphs.Dict.TryGetValue(assetGuid, out var match))
                match = playerHolder.Player.Body.Morphs.AddNew(Current.Prefab.AssetGUID,
                    Current.AvatarBodyShapes.AddToCharacter());
            return match;
        }

        public void Setup() => sliders.Setup(GetMatch(), Current);

        void SortBody() => sliders.SetupOfType(GetMatch(), Current,
            CharacterAvatar.BodyShapes.BodyShapeTypes.Body);

        void SortFace() => sliders.SetupOfType(GetMatch(), Current,
            CharacterAvatar.BodyShapes.BodyShapeTypes.Face);
    }
}