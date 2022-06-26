using System.Collections.Generic;
using System.Linq;
using Character;
using UnityEngine;

namespace Safe_To_Share.Scripts.AfterBattle
{
    [CreateAssetMenu(fileName = "SexActsDict", menuName = "AfterBattle/SexActsDict")]
    public class SexActionsDict : ScriptableObject
    {
        [SerializeField] List<SexAction> sexActs = new();
        [SerializeField] List<EssenceAction> drainActions = new();
        [SerializeField] List<VoreAction> voreActions = new();
        public IEnumerable<SexAction> SexActs => sexActs;

        public IEnumerable<EssenceAction> DrainActions => drainActions;

        public IEnumerable<VoreAction> VoreActions => voreActions;

        public IEnumerable<SexAction> SexActsWeCanDo(BaseCharacter me, BaseCharacter partner)
            => SexActs.Where(sexAction => sexAction.CanUse(me, partner));

        public IEnumerable<EssenceAction> DrainActionsWeCanDo(BaseCharacter me, BaseCharacter partner)
            => DrainActions.Where(drainAction => drainAction.CanUse(me, partner));

        public IEnumerable<VoreAction> VoreActionsWeCanDo(BaseCharacter pred, BaseCharacter prey)
            => VoreActions.Where(voreAction => voreAction.CanUse(pred, prey));
    }
}