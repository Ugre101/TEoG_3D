using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GameUIAndMenus
{
    public class ExpBar : GameMenu
    {
        [SerializeField] Slider bar;

        [SerializeField] TextMeshProUGUI expHave, expNeed;

        // Start is called before the first frame update
        void OnEnable()
        {
            Setup();
            Bind();
        }


        void OnDisable() => UnBind();

        public override bool BlockIfActive() => false;

        void Bind()
        {
            holder.RePlaced += Setup;
            Player.LevelSystem.ExpGained += UpdateExp;
            Player.LevelSystem.LevelGained += UpdateNeeded;
        }

        void UnBind()
        {
            holder.RePlaced -= Setup;
            Player.LevelSystem.ExpGained -= UpdateExp;
            Player.LevelSystem.LevelGained -= UpdateNeeded;
        }

        void UpdateNeeded(int obj) => expNeed.text = Player.LevelSystem.ExpNeeded.ToString();
        void UpdateExp(int obj) => expHave.text = obj.ToString();

        void Setup()
        {
            var levelSys = Player.LevelSystem;
            bar.value = (float)levelSys.Exp / levelSys.ExpNeeded;
            UpdateExp(levelSys.Exp);
            UpdateNeeded(levelSys.ExpNeeded);
        }
    }
}