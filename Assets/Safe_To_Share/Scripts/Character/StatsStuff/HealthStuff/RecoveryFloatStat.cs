using System;
using Safe_to_Share.Scripts.CustomClasses;
using UnityEngine;

namespace Character.StatsStuff.HealthStuff
{
    [Serializable]
    public class RecoveryFloatStat : BaseConstFloatStat, ITickMinute
    {
        [SerializeField] float currentValue;
        [SerializeField] BaseFloatStat recovery;

        public RecoveryFloatStat(float baseValue, BaseFloatStat recovery) : base(baseValue) => this.recovery = recovery;

        public BaseFloatStat Recovery => recovery;

        public bool Dead { get; private set; }

        public float CurrentValue
        {
            get => currentValue;
            protected set
            {
                Dead = value <= 0;
                if (value < currentValue)
                    ValueDecrease?.Invoke(currentValue - value);
                else
                    ValueIncrease?.Invoke(value - currentValue);
                currentValue = Mathf.Clamp(value, 0, Value);
                CurrentValueChange?.Invoke(CurrentValue);
            }
        }

        public override bool Dirty
        {
            get => base.Dirty;
            protected set
            {
                base.Dirty = value;
                if (value)
                    MaxValueChange?.Invoke(Value);
            }
        }

        public void TickMin(int ticks = 1)
        {
            if (CurrentValue >= Value)
                return;
            // % of max health heal per tick
            CurrentValue += Value * (Recovery.Value / 100f) * ticks;
        }

        public event Action<float> MaxValueChange, CurrentValueChange;
        public event Action<float> ValueIncrease, ValueDecrease;

        public bool DecreaseCurrentValue(float dmg) => (CurrentValue -= dmg) <= 0;

        public void IncreaseCurrentValue(float heal) => CurrentValue += heal;

        public void FullRecovery() => CurrentValue = Value;

        public void Refresh()
        {
            MaxValueChange?.Invoke(Value);
            CurrentValueChange?.Invoke(CurrentValue);
        }
    }
}