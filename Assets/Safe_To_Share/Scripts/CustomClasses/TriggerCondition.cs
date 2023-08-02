using UnityEngine;

namespace Safe_To_Share.Scripts.CustomClasses {
    public class TriggerCondition: MonoBehaviour{
        public virtual bool ShouldTrigger() {
            return false;
        }
    }
}