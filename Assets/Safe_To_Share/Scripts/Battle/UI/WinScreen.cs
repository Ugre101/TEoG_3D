using System.Collections;
using Battle;
using Safe_To_Share.Scripts.Options;
using UnityEngine;
using UnityEngine.UI;

namespace Safe_To_Share.Scripts.Battle.UI {
    public sealed class WinScreen : MonoBehaviour {
        [SerializeField] Button leave, fuck;


        readonly WaitForSeconds waitForSeconds = new(0.2f);

        void Start() {
            var skip = PlayerPrefs.GetInt(ToggleGoDirectlyToAfterBattle.SkipVictoryScreen) == 1;
            if (skip)
                StartCoroutine(SmallDelay());

            leave.onClick.AddListener(Leave);
            fuck.onClick.AddListener(GoToAfterBattle);
        }

        IEnumerator SmallDelay() {
            leave.gameObject.SetActive(false);
            fuck.gameObject.SetActive(false);
            yield return waitForSeconds;
            GoToAfterBattle();
        }

        void Leave() {
            BattleManager.Instance.Leave();
            leave.gameObject.SetActive(false);
        }

        void GoToAfterBattle() {
            BattleManager.Instance.GoToAfterBattle();
            fuck.gameObject.SetActive(false);
        }
    }
}