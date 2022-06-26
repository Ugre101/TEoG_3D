
using System;
using UnityEngine;

namespace Character
{
    [Serializable]
    public class Hair
    {
        [SerializeField] bool bald;
        [SerializeField] Color color;
        public Hair(bool b, Color getColor)
        {
            bald = b;
            color = getColor;
        }

        public bool Bald
        {
            get => bald;
            set => bald = value;
        }

        public Color HairColor
        {
            get => color;
            set => color = value;
        }
    }
}