using UnityEngine;

namespace Safe_To_Share.Scripts.AfterBattle
{
    public class AfterBattleAvatarScaler : MonoBehaviour
    {
        [SerializeField] float minSize = 0.5f, maxSize = 2f;
        float originalHeight = 160f;
        public void NewHeight(float avatarHeight) => originalHeight = avatarHeight;

        public void ChangeScale(float newHeight)
        {
            float f = newHeight / originalHeight;
            f = Mathf.Clamp(f, minSize, maxSize);
            transform.localScale = new Vector3(f, f, f);
        }
    }
}