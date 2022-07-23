using System.Collections;
using Options;
using UnityEngine;
using UnityEngine.UI;

namespace Battle.UI
{
    public class WinScreen : MonoBehaviour
    {
        [SerializeField] Button leave, fuck;


        readonly WaitForSeconds waitForSeconds = new(0.2f);

        void Start()
        {
            bool skip = PlayerPrefs.GetInt(ToggleGoDirectlyToAfterBattle.SkipVictoryScreen) == 1;
            if (skip)
                StartCoroutine(SmallDelay());

            leave.onClick.AddListener(Leave);
            fuck.onClick.AddListener(GoToAfterBattle);
        }

        IEnumerator SmallDelay()
        {
            leave.gameObject.SetActive(false);
            fuck.gameObject.SetActive(false);
            yield return waitForSeconds;
            GoToAfterBattle();
        }

        void Leave()
        {
            BattleManager.Instance.Leave();
            leave.gameObject.SetActive(false);
        }

        void GoToAfterBattle()
        {
            BattleManager.Instance.GoToAfterBattle();
            fuck.gameObject.SetActive(false);
        }
    }
}