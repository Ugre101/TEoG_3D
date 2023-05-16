using Battle;
using UnityEngine;
using UnityEngine.UI;

namespace Safe_To_Share.Scripts.Battle.UI
{
    public sealed class DefeatScreen : MonoBehaviour
    {
        [SerializeField] Button leaveBtn;

        // Start is called before the first frame update
        void Start() => leaveBtn.onClick.AddListener(Leave);

        static void Leave() => BattleManager.Instance.GoToDefeat();
    }
}