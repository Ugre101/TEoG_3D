using UnityEngine;

namespace Safe_To_Share.Scripts.GameUIAndMenus.GameOptions
{
    public class GameOptionsMenu : MonoBehaviour
    {
        [SerializeField] GameObject renameGenders;

        void OnEnable() => renameGenders.SetActive(false);
    }
}