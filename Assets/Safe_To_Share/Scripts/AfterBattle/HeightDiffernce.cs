using Character.BodyStuff;
using UnityEngine;

namespace Safe_To_Share.Scripts.AfterBattle
{
    public class HeightDiffernce : MonoBehaviour
    {
        [SerializeField] AfterBattleAvatarScaler player, partner;

        public void SetHeights(Body actor1, Body actor2)
        {
            float actor1Factor = actor1.Height.Value / actor2.Height.Value;
            float actor2Factor = actor2.Height.Value / actor1.Height.Value;
            player.ChangeScale(actor1Factor);
            partner.ChangeScale(actor2Factor);
        }
    }
}