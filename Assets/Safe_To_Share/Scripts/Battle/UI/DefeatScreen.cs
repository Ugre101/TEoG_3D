using UnityEngine;
using UnityEngine.UI;

namespace Battle.UI
{
    public class DefeatScreen : MonoBehaviour
    {
        [SerializeField] Button leaveBtn;

        readonly WaitForSeconds waitForSeconds = new(0.2f);

        // Start is called before the first frame update
        void Start() => leaveBtn.onClick.AddListener(Leave);

        static void Leave() => BattleManager.Instance.GoToDefeat();
    }
}