using UnityEngine;

namespace GameUIAndMenus.GameOptions
{
    public class GameOptionsMenu : MonoBehaviour
    {
        [SerializeField] GameObject renameGenders;

        void OnEnable() => renameGenders.SetActive(false);
    }
}