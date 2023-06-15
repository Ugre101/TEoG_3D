using System;
using System.Collections.Generic;
using UnityEngine;

namespace Safe_To_Share.Scripts.Character.TempEffects {
    [Serializable]
    public class TempEffects {
        [field: SerializeField] public List<TempEffect> Effects = new();

        public void AddTempEffect(string source, int hours, SourceType sourceType) {
            foreach (var effect in Effects) {
                if (effect.Source != source) continue;
                effect.AddHours(hours);
                return;
            }

            Effects.Add(new TempEffect(source, hours, sourceType));
        }

        public IEnumerable<string> TickDown(int ticks) {
            foreach (var effect in Effects)
                if (effect.TickDown(ticks))
                    yield return effect.Source;
        }
    }
}