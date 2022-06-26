using AvatarStuff.Holders;
using Character;
using Character.PlayerStuff;
using UnityEngine;
using UnityEngine.UI;

namespace AvatarStuff.UI
{
    public class BodyMorphPanel : MonoBehaviour
    {
        [SerializeField] BodyMorphSliders sliders;
        [Header("Sorting"), SerializeField,] Button all;
        [SerializeField] Button body, face;
        PlayerHolder playerHolder;

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
            var assetGuid = playerHolder.CurrentAvatar.Prefab.AssetGUID;
            if (!playerHolder.Player.Body.Morphs.Dict.TryGetValue(assetGuid, out var match))
                match = playerHolder.Player.Body.Morphs.AddNew(playerHolder.CurrentAvatar.Prefab.AssetGUID,playerHolder.CurrentAvatar.AvatarBodyShapes.AddToCharacter());
            return match;
        }

        public void Setup() => sliders.Setup(GetMatch(), playerHolder.CurrentAvatar);

        void SortBody() => sliders.SetupOfType(GetMatch(), playerHolder.CurrentAvatar,
            CharacterAvatar.BodyShapes.BodyShapeTypes.Body);

        void SortFace() => sliders.SetupOfType(GetMatch(), playerHolder.CurrentAvatar,
            CharacterAvatar.BodyShapes.BodyShapeTypes.Face);
    }
}