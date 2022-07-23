using System;
using Character;
using DormAndHome.Dorm;
using UnityEngine;

namespace Dialogue.DialogueActions
{
    [Serializable]
    public class AddToDorm : DialogueBaseAction
    {
        [SerializeField] bool needSpace;

        public override bool MeetsCondition() => !needSpace || DormManager.Instance.DormHasSpace;

        public override void Invoke(BaseCharacter toAdd) => DormManager.Instance.AddToDorm(toAdd);
    }
}