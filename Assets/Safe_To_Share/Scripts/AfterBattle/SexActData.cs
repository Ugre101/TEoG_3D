using System;
using System.Collections.Generic;
using Safe_To_Share.Scripts.AfterBattle.Animation;
using Safe_To_Share.Scripts.AfterBattle.Vore;
using UnityEngine;

namespace Safe_To_Share.Scripts.AfterBattle
{
    [Serializable]
    public struct SexActData
    {
        
        [field: SerializeField] public SexAnimationFilterList SexAnimationFilterList { get; private set; }
        [SerializeField] string text;
        [SerializeField] List<string> afterText;
        public List<string> AfterText => afterText;

        public string TitleText => text;
    }
}