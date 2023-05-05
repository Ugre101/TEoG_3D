using System.Collections.Generic;
using System.Linq;
using AvatarStuff.Holders;
using Character;
using Character.BodyStuff;
using Character.EssenceStuff;
using Character.Organs;
using Character.StatsStuff.Mods;
using Items;
using Safe_To_Share.Scripts.Holders;
using UnityEngine;
using Random = System.Random;

namespace Safe_To_Share.Scripts.Special_Items
{
    [CreateAssetMenu(menuName = "Items/Special Items/Create Experimental Tonic", fileName = "ExperimentalTonic",
        order = 0)]
    public class ExperimentalTonic : Item
    {
        readonly RandomEffect[] randomEffects =
        {
            new Shrink(1),
            new Grow(2),
            new GrowOrgan(2),
            new TempMini(2),
            new TempFat(1),
            new TempSwole(1),
            new TempShrinkOrgans(1),
            new GainStat(1),
        };

        public override void Use(BaseCharacter user)
        {
            float tot = randomEffects.Sum(re => re.Weight);
            float rng = UnityEngine.Random.Range(0, tot + 1);
            RandomEffect chosen = GetARandomEffect(rng);
            chosen?.Use(user, Guid);
        }

        RandomEffect GetARandomEffect(float rng)
        {
            int weight = 0;
            foreach (RandomEffect randomEffect in randomEffects)
            {
                weight += randomEffect.Weight;
                if (rng <= weight)
                    return randomEffect;
            }

            return null;
        }

        abstract class RandomEffect
        {
            protected RandomEffect(int weight) => Weight = weight;

            public int Weight { get; }
            public abstract void Use(BaseCharacter user, string itemGuid);
        }

        class Shrink : RandomEffect
        {
            public Shrink(int weight) : base(weight)
            {
            }

            public override void Use(BaseCharacter user, string itemGuid) => user.Body.ShrinkBodyByPercent(10);
        }

        class Grow : RandomEffect
        {
            public Grow(int weight) : base(weight)
            {
            }

            public override void Use(BaseCharacter user, string itemGuid) => user.Body.GrowBodyByPercent(5);
        }

        class GrowOrgan : RandomEffect
        {
            readonly Random rng = new();

            public GrowOrgan(int weight) : base(weight)
            {
            }

            public override void Use(BaseCharacter user, string itemGuid)
            {
                List<BaseOrgan> organs = new();
                organs.AddRange(user.SexualOrgans.Balls.BaseList);
                organs.AddRange(user.SexualOrgans.Dicks.BaseList);
                organs.AddRange(user.SexualOrgans.Boobs.BaseList);
                organs.AddRange(user.SexualOrgans.Vaginas.BaseList);
                if (organs.Count > 0)
                    for (int i = 0; i < UnityEngine.Random.Range(1, 6); i++)
                        organs[rng.Next(organs.Count)].BaseValue++;
            }
        }

        class TempMini : RandomEffect
        {
            public TempMini(int weight) : base(weight)
            {
            }

            public override void Use(BaseCharacter user, string itemGuid) =>
                user.Body.Height.Mods.AddTempStatMod(1, -80, itemGuid, ModType.Percent);
        }

        class TempShrinkOrgans : RandomEffect
        {
            readonly Random rng = new();

            public TempShrinkOrgans(int weight) : base(weight)
            {
            }

            public override void Use(BaseCharacter user, string itemGuid)
            {
                List<BaseOrgan> organs = new();
                organs.AddRange(user.SexualOrgans.Balls.BaseList);
                organs.AddRange(user.SexualOrgans.Dicks.BaseList);
                organs.AddRange(user.SexualOrgans.Boobs.BaseList);
                organs.AddRange(user.SexualOrgans.Vaginas.BaseList);
                if (organs.Count > 0)
                    for (int i = 0; i < 13; i++)
                    {
                        BaseOrgan baseOrgan = organs[rng.Next(organs.Count)];
                        baseOrgan.Mods.AddTempStatMod(2, -50, itemGuid, ModType.Percent);
                        organs.Remove(baseOrgan);
                        if (organs.Count == 0)
                            break;
                    }
            }
        }

        class TempSwole : RandomEffect
        {
            public TempSwole(int weight) : base(weight)
            {
            }

            public override void Use(BaseCharacter user, string itemGuid) =>
                user.Body.Muscle.Mods.AddTempStatMod(12, 30, itemGuid, ModType.Percent);
        }

        class TempFat : RandomEffect
        {
            public TempFat(int weight) : base(weight)
            {
            }

            public override void Use(BaseCharacter user, string itemGuid) =>
                user.Body.Fat.Mods.AddTempStatMod(8, 22, itemGuid, ModType.Flat);
        }

        class SecretIsland : RandomEffect
        {
            // Small hidden island  
            public SecretIsland(int weight) : base(weight)
            {
            }

            public override void Use(BaseCharacter user, string itemGuid)
            {
            }
        }

        class GainStat : RandomEffect
        {
            public GainStat(int weight) : base(weight)
            {
            }

            public override void Use(BaseCharacter user, string itemGuid)
            {
                int rng = UnityEngine.Random.Range(0, 5);
                switch (rng)
                {
                    case 0:
                        user.Stats.Agility.BaseValue++;
                        break;
                    case 1:
                        user.Stats.Charisma.BaseValue++;
                        break;
                    case 2:
                        user.Stats.Constitution.BaseValue++;
                        break;
                    case 3:
                        user.Stats.Intelligence.BaseValue++;
                        break;
                    case 4:
                        user.Stats.Strength.BaseValue++;
                        break;
                }
            }
        }

        class SelfDrain : RandomEffect
        {
            public SelfDrain(int weight) : base(weight)
            {
            }

            public override void Use(BaseCharacter user, string itemGuid)
            {
                ChangeLog log = new();
                if (UnityEngine.Random.value > 0.5f)
                    user.GainFemi(user.LoseMasc(user.DrainAmount(user), log));
                else
                    user.GainMasc(user.LoseFemi(user.DrainAmount(user), log));
            }
        }

        class BadGas : RandomEffect
        {
            public BadGas(int weight) : base(weight)
            {
            }

            public override void Use(BaseCharacter user, string itemGuid)
            {
                var holder = PlayerHolder.Instance;
                if (holder != null && holder.TryGetComponent(out Rigidbody rigidbody))
                    rigidbody.AddForce(Vector3.up * 12f, ForceMode.Impulse);
            }
        }
    }
}