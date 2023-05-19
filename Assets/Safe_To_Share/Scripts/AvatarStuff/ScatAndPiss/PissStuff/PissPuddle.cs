using System;
using System.Collections.Generic;
using UnityEngine;

namespace Safe_To_Share.Scripts.AvatarStuff.ScatAndPiss
{

    public sealed class PissPuddle : MonoBehaviour
    {
        public static List<PissPuddle> ExistingPuddles { get; } = new();

        float time;
        [SerializeField,Range(float.Epsilon,0.001f)] float growthRate = 0.01f;

        void Update()
        {
            time += Time.deltaTime;
            if (30 < time)
                Destroy(gameObject);
        }

        void OnDestroy()
        {
            ExistingPuddles.Remove(this);
        }

        public void Grow()
        {
            var rate = growthRate / transform.localScale.y;
            transform.localScale += new Vector3(rate, rate  / 10, rate);
            time = 0;
        }
    }
}