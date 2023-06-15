using System;
using Safe_to_Share.Scripts.CustomClasses;
using UnityEngine;

namespace Character.StatsStuff.HealthStuff {
    [Serializable]
    public class RecoveryIntStat : BaseConstIntStat, ITickMinute, ITickHour {
        [SerializeField] int currentValue;
        public RecoveryIntStat(int baseValue, IntRecovery intRecovery) : base(baseValue) => IntRecovery = intRecovery;

        public IntRecovery IntRecovery { get; }

        public bool Dead { get; private set; }

        public int CurrentValue {
            get => currentValue;
            private set {
                Dead = value <= 0;
                if (value < currentValue)
                    ValueDecrease?.Invoke(currentValue - value);
                else
                    ValueIncrease?.Invoke(value - currentValue);
                currentValue = Mathf.Clamp(value, 0, Value);
                CurrentValueChange?.Invoke(CurrentValue);
            }
        }

        public bool TickHour(int ticks = 1) => Mods.TickHour(ticks) | IntRecovery.Mods.TickHour(ticks);

        public void TickMin(int ticks = 1) {
            if (CurrentValue >= Value && IntRecovery.Value >= 0)
                return;
            // % of max health heal per tick
            CurrentValue += Mathf.CeilToInt(Value * (IntRecovery.Value / 100f) * ticks);
        }

        public event Action<int> MaxValueChange, CurrentValueChange;
        public event Action<int> ValueIncrease, ValueDecrease;

        public bool DecreaseCurrentValue(int dmg) => (CurrentValue -= dmg) <= 0;

        public void IncreaseCurrentValue(int heal) => CurrentValue += heal;

        public void FullRecovery(int percent = 100) {
            if (percent == 100) {
                CurrentValue = Value;
                return;
            }

            var want = Mathf.RoundToInt(Value * (percent / 100f));
            if (CurrentValue < want)
                CurrentValue = want;
        }

        public void Refresh() {
            MaxValueChange?.Invoke(Value);
            CurrentValueChange?.Invoke(CurrentValue);
        }

        protected void InvokeMaxChange() => MaxValueChange?.Invoke(Value);
    }
}