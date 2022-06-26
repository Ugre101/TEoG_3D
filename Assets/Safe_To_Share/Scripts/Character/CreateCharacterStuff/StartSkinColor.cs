using System;
using UnityEngine;

namespace Character.CreateCharacterStuff
{
    [Serializable]
    public struct StartSkinColor
    {
        [SerializeField] bool clampRandomColor;
        [SerializeField, Range(0f, 0.5f)] float whitest;
        [SerializeField, Range(0.5f, 1f)] float darkest;
        [SerializeField] bool manualSkinColor;
        [SerializeField, Range(0f, 1f)] float skinDarkness;

        public float GetSkinDarkness()
        {
            if (manualSkinColor)
                return skinDarkness;
            return clampRandomColor ? UnityEngine.Random.Range(whitest, darkest) : UnityEngine.Random.value;
        }
    }
}