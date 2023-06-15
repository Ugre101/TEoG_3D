using Character;
using Character.PlayerStuff;

namespace Dialogue {
    public sealed class StartBattleDialogue : DialogueBaseNode {
        public void StartBattle(Player player, BaseCharacter[] enemies, BaseCharacter[] allies = null) =>
            player.InvokeCombat(enemies);
    }
}