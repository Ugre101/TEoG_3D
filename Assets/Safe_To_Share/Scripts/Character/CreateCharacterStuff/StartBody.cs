using System;
using Character.BodyStuff;
using Character.IslandData;
using Safe_to_Share.Scripts.CustomClasses;
using UnityEngine;

namespace Character.CreateCharacterStuff
{
    [Serializable]
    public struct StartBody
    {
        [SerializeField] int height;
        [SerializeField] int muscle;
        [SerializeField] int fat;
        [SerializeField] RngValue rng;
        public void Default()
        {
            height = 160;
            muscle = 20;
            fat = 10;
        }

        int GetValue(int value) => Mathf.RoundToInt(value * rng.GetRandomValue);

        public Body NewBody() => new(GetValue(muscle), GetValue(fat), GetValue(height));

        public Body NewBody(Islands islands) => IslandStonesDatas.IslandDataDict.TryGetValue(islands, out var data)
                ? new Body(GetValue(muscle) + data.bodyData.muscle.Current, GetValue(fat) + data.bodyData.fat.Current, GetValue(height) + data.bodyData.height.Current)
                : NewBody();
        
    }
}