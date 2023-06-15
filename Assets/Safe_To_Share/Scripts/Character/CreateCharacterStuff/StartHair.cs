using System;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

namespace Character.CreateCharacterStuff {
    [Serializable]
    public class StartHair {
        [SerializeField] bool bald;
        [SerializeField] List<Color> hairColors = new() { Color.white, };

        Random rng = new();

        public Hair GetHair() => new(bald, GetColor());

        Color GetColor() {
            if (hairColors == null || hairColors.Count == 0)
                return Color.white;
            return hairColors[rng.Next(hairColors.Count)];
        }
    }
}