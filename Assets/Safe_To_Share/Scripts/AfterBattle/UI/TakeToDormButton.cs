using System;
using Character;
using Character.EnemyStuff;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Safe_To_Share.Scripts.AfterBattle.UI {
    public sealed class TakeToDormButton : MonoBehaviour {
        [SerializeField] Button btn;
        [SerializeField] Image pie;
        [SerializeField] TextMeshProUGUI left;

        void Start() => btn.onClick.AddListener(Take);
        public static event Action TakeToDorm;

        public void ShowOrgasmsLeft(BaseCharacter wantToTake) {
            if (wantToTake is not Enemy enemy) {
                gameObject.SetActive(false);
                return;
            }

            gameObject.SetActive(true);
            var percent = Mathf.Min(1f, (float)enemy.SexStats.TotalOrgasms / enemy.CanTake.OrgasmsNeeded);
            pie.fillAmount = 1f - percent;
            var orgsLeft = enemy.CanTake.OrgasmsNeeded - enemy.SexStats.TotalOrgasms;
            left.text = orgsLeft <= 0 ? string.Empty : orgsLeft.ToString();
        }

        static void Take() => TakeToDorm?.Invoke();
    }
}