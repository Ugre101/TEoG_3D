using UnityEngine;
using UnityEngine.UI;

namespace Battle
{
    public class WinScreenSfw : MonoBehaviour
    {
        [SerializeField] Button leave;

        // Start is called before the first frame update
        void Start() => leave.onClick.AddListener(Leave);

        void Leave()
        {
            leave.gameObject.SetActive(false);
            BattleManager.Instance.Leave();
        }
    }
}