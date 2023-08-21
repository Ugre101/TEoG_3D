using Character.PlayerStuff;
using TMPro;
using UnityEngine;

namespace Safe_To_Share.Scripts.Shrines {
    public abstract class BaseShrine : MonoBehaviour {
        [SerializeField] TextMeshProUGUI blessingPoints;
        protected abstract ShrinePoints Points { get; }

        public static BaseShrine LastLoadedShrine;

        void Start() {
            LastLoadedShrine = this;
        }

        public virtual void EnterShrine(Player player) {
            blessingPoints.text = Points.BlessingPoints.ToString();
        }
    }
}