using UnityEngine;

namespace Safe_To_Share.Scripts.AfterBattle {
    public sealed class AfterBattleAvatarScaler : MonoBehaviour {
        static float playerHeight, enemyHeight;
        [SerializeField] float minSize = 0.5f, maxSize = 2f;

        float originalHeight = 160f;

        public float Height { get; private set; } = 1f;

        public void NewHeight(float avatarHeight) => originalHeight = avatarHeight;

        public void ChangeScale(float newHeight, bool playerAvatar) {
            if (playerAvatar)
                playerHeight = newHeight;
            else
                enemyHeight = newHeight;
            var f = newHeight / originalHeight;
            f = Mathf.Clamp(f, minSize, maxSize);
            Height = f;
            transform.localScale = new Vector3(f, f, f);
            // TODO make it so two Tall/Short avatars still show height difference
        }

        public void SetHeight(float bodyHeight, bool playerAvatar) {
            if (playerAvatar)
                playerHeight = bodyHeight;
            else
                enemyHeight = bodyHeight;
        }
    }
}