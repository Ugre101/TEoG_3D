using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Safe_To_Share.Scripts.GameUIAndMenus {
    public sealed class ExpBar : GameMenu {
        [SerializeField] Slider bar;

        [SerializeField] TextMeshProUGUI expHave, expNeed;

        // Start is called before the first frame update
        void OnEnable() {
            Setup();
            holder.RePlaced += Setup;
        }


        void OnDisable() => UnBind();

        public override bool BlockIfActive() => false;

        void UnBind() {
            holder.RePlaced -= Setup;
            Player.LevelSystem.ExpGained -= UpdateExp;
            Player.LevelSystem.LevelGained -= UpdateNeeded;
        }

        void UpdateNeeded(int obj) => expNeed.text = Player.LevelSystem.ExpNeeded.ToString();
        void UpdateExp(int obj) => expHave.text = obj.ToString();

        void Setup() {
            Player.LevelSystem.ExpGained += UpdateExp;
            Player.LevelSystem.LevelGained += UpdateNeeded;
            var levelSys = Player.LevelSystem;
            bar.value = (float)levelSys.Exp / levelSys.ExpNeeded;
            UpdateExp(levelSys.Exp);
            UpdateNeeded(levelSys.ExpNeeded);
        }
    }
}