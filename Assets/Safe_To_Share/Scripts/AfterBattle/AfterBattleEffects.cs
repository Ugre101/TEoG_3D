using System.Collections;
using UnityEngine;

namespace Safe_To_Share.Scripts.AfterBattle
{
    public sealed class AfterBattleEffects : MonoBehaviour
    {
        [SerializeField] ParticleSystem drainedMasc;
        readonly WaitForSeconds waitForSeconds = new(2f);


        public void DrainedMasc()
        {
            drainedMasc.gameObject.SetActive(true);
            drainedMasc.Play();
            StartCoroutine(DelayedTurnOf());
        }

        IEnumerator DelayedTurnOf()
        {
            yield return waitForSeconds;
            drainedMasc.Stop();
            drainedMasc.gameObject.SetActive(false);
        }
    }
}