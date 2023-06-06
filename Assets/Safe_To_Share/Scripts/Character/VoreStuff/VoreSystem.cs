using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Character.EssenceStuff;
using Character.Organs;
using Character.Organs.OrgansContainers;
using Character.StatsStuff.Mods;
using Character.VoreStuff.VorePerks;
using CustomClasses;
using Safe_To_Share.Scripts.Character.VoreStuff.VoreDigestionModes;
using Safe_To_Share.Scripts.Character.VoreStuff.VoreDigestionModes.Balls;
using Safe_To_Share.Scripts.Character.VoreStuff.VoreDigestionModes.Boobs;
using Safe_To_Share.Scripts.Character.VoreStuff.VoreDigestionModes.Cook;
using Safe_To_Share.Scripts.Character.VoreStuff.VoreDigestionModes.Vagina;
using Safe_to_Share.Scripts.CustomClasses;
using Safe_To_Share.Scripts.Static;
using UnityEngine;

namespace Character.VoreStuff
{
    [Serializable]
    public class VoreSystem
    {
        [SerializeField] VoreLevel level = new();
        [SerializeField] VoreOrgan stomach = new();
        [SerializeField] StomachDigestionMode stomachDigestionMode = new();
        [SerializeField] BallsDigestionModes ballsDigest = new();
        [SerializeField] CockDigestionModes cockDigest = new();
        [SerializeField] BoobsDigestionModes boobsDigest = new();
        [SerializeField] AnalDigestionModes analDigest = new();
        [SerializeField] VaginaDigestionModes vaginaDigest = new();
        [SerializeField] DrainEssenceType drainEssenceType = DrainEssenceType.Both;
        public BaseConstIntStat digestionStrength = new(1);
        public BaseConstIntStat pleasureDigestion = new(0);
        public BaseConstIntStat orgasmDrain = new(0);
        public ModsContainer capacityBoost = new();

        readonly Dictionary<string, VorePerkNewDigestionMode> perkOnes = new();

        Dictionary<SexualOrganType, VoreOrganDigestionMode> voreOrgans;

        public bool CanPleasurePreys { get; private set; }

        public VoreLevel Level => level;

        public VoreOrgan Stomach => stomach;

        public Dictionary<SexualOrganType, VoreOrganDigestionMode> VoreOrgans =>
            voreOrgans ??= new Dictionary<SexualOrganType, VoreOrganDigestionMode>
            {
                { SexualOrganType.Balls, ballsDigest },
                { SexualOrganType.Dick, cockDigest },
                { SexualOrganType.Boobs, boobsDigest },
                { SexualOrganType.Anal, analDigest },
                { SexualOrganType.Vagina, vaginaDigest },
            };

        public DrainEssenceType DrainEssenceType
        {
            get => drainEssenceType;
            set => drainEssenceType = value;
        }

        public StomachDigestionMode StomachDigestionMode => stomachDigestionMode;

        public bool TickHour(BaseCharacter pred, int ticks = 1)
        {
            TickPreyHour(pred, ticks);
            return digestionStrength.Mods.TickHour(ticks) |
                   pleasureDigestion.Mods.TickHour(ticks) |
                   orgasmDrain.Mods.TickHour(ticks) |
                   capacityBoost.TickHour(ticks);
        }

        void TickPreyHour(BaseCharacter pred, int ticks = 1)
        {
            stomach.TickHour(ticks);
            foreach (var (type, mode) in VoreOrgans)
                if (pred.SexualOrgans.Containers.TryGetValue(type, out var container))
                    foreach (var baseOrgan in container.BaseList)
                        baseOrgan.Vore.TickHour(ticks);
        }

        VoreOrganDigestionMode GetDigestionMode(VoreType type) => type switch
        {
            VoreType.Oral => stomachDigestionMode,
            VoreType.Balls => ballsDigest,
            VoreType.UnBirth => vaginaDigest,
            VoreType.Anal => analDigest,
            VoreType.Breast => boobsDigest,
            VoreType.Cock => cockDigest,
            _ => throw new ArgumentOutOfRangeException(nameof(type), type, null),
        };

        public static event Action<int> Digested;
        public static void HaveDigested(int id) => Digested?.Invoke(id);

        public bool VoreTick(BaseCharacter pred, bool predIsPlayer, int ticks)
        {
            bool change = false;
            CanPleasurePreys = pleasureDigestion.Value > 0;
            for (int i = 0; i < ticks; i++)
            {
                foreach (VorePerk vorePerk in Level.OwnedPerks)
                    vorePerk.OnTick(pred);
                if (OrganContainersTick(pred, predIsPlayer))
                    change = true;
                if (StomachTick(pred, predIsPlayer))
                    change = true;
            }

            return change;
        }

        bool StomachTick(BaseCharacter pred, bool predIsPlayer)
        {
            if (stomach.PreysIds.Count <= 0)
                return false;
            bool change = false;
            stomach.SetStretch(VoreSystemExtension.OralVoreCapacity(pred));
            if (stomach.Stretch > 1f)
            {
                RegurgitateStomachPrey(pred);
                change = true;
            }

            PleasureDigestion(pred, stomach.PreysIds);
            if (StomachDigestionMode.DigestionMethod.Tick(pred, Stomach, predIsPlayer))
                change = true;
            Level.GainExp(Mathf.RoundToInt(10 * stomach.Stretch));
            return change;
        }

        void RegurgitateStomachPrey(BaseCharacter pred)
        {
            Prey toReg = FindLeastDigested(stomach.PreysIds);
            if (toReg == null || !stomach.PreysIds.Remove(toReg.Identity.ID))
                return;
            string log =
                $"{pred.Identity.FirstName}'s stomach was full and had to regurgitate {toReg.Identity.FirstName}";
            EventLog.AddEvent(log);
        }

        bool OrganContainersTick(BaseCharacter pred, bool predIsPlayer)
        {
            bool change = false;
            foreach (var organDigestionMode in VoreOrgans)
                if (TickOrganContainer(pred, organDigestionMode, predIsPlayer))
                    change = true;
            return change;
        }

        bool TickOrganContainer(BaseCharacter pred,
            KeyValuePair<SexualOrganType, VoreOrganDigestionMode> organDigestionMode, bool predIsPlayer)
        {
            if (!pred.SexualOrgans.Containers.TryGetValue(organDigestionMode.Key, out BaseOrgansContainer container))
                return false;
            bool change = false;
            foreach (BaseOrgan baseOrgan in container.BaseList.TakeWhile(OrganHasPreys))
                if (TickOrgan(pred, organDigestionMode, baseOrgan, predIsPlayer))
                    change = true;
            return change;
        }

        static bool OrganHasPreys(BaseOrgan baseOrgan) =>
            baseOrgan.Vore.PreysIds.Count != 0 || baseOrgan.Vore.SpecialPreysIds.Count != 0;

        bool TickOrgan(BaseCharacter pred, KeyValuePair<SexualOrganType, VoreOrganDigestionMode> organDigestionMode,
            BaseOrgan baseOrgan, bool predIsPlayer)
        {
            bool change = false;
            baseOrgan.Vore.SetStretch(VoreSystemExtension.OrganVoreCapacity(pred, baseOrgan, organDigestionMode.Key));
            if (HandleOrganRegurgitation(pred, nameof(organDigestionMode.Key), baseOrgan))
                change = true;
            PleasureSexualOrganDigestion(pred, baseOrgan, organDigestionMode.Value.DigestionMethod);
            organDigestionMode.Value.DigestionMethod.Tick(pred, baseOrgan, predIsPlayer);
            Level.GainExp(Mathf.RoundToInt(5 * baseOrgan.Vore.Stretch));
            HandleSpecialSexOrganDigestion(pred, organDigestionMode.Key, baseOrgan, predIsPlayer);
            return change;
        }

        void PleasureSexualOrganDigestion(BaseCharacter pred, BaseOrgan baseOrgan, DigestionMethod digestionMode)
        {
            if (!CanPleasurePreys)
                return;
            foreach (int preysId in baseOrgan.Vore.PreysIds)
                CheckIfOrgasm(preysId);

            void CheckIfOrgasm(int preyId)
            {
                if (!VoredCharacters.PreyDict.TryGetValue(preyId, out Prey prey))
                    return;
                int orgasms = prey.SexStats.GainArousal(pleasureDigestion.Value);
                if (orgasms <= 0)
                    return;
                digestionMode.OnPreyOrgasmInSexualOrgan(pred, baseOrgan, prey, orgasms);
                pred.DrainEssenceOfType(prey, DrainEssenceType, orgasmDrain.Value * orgasms);
            }
        }


        void HandleSpecialSexOrganDigestion(BaseCharacter pred, SexualOrganType key, BaseOrgan baseOrgan,
            bool predIsPlayer)
        {
            for (int index = baseOrgan.Vore.SpecialPreysIds.Count; index-- > 0;)
            {
                var preyId = baseOrgan.Vore.SpecialPreysIds[index];
                TickSpecialSexOrganPrey(pred, key, baseOrgan, preyId, predIsPlayer);
            }
        }

        void TickSpecialSexOrganPrey(BaseCharacter pred, SexualOrganType key, BaseOrgan baseOrgan, int preyId,
            bool predIsPlayer)
        {
            if (!VoredCharacters.PreyDict.TryGetValue(preyId, out var prey))
                return;
            if (perkOnes.TryGetValue(prey.SpecialDigestion, out var perk))
                perk.SpecialOrganDigestion(pred, baseOrgan, key, preyId, predIsPlayer);
            else if (!(Level.OwnedPerks.OfType<VorePerkNewDigestionMode>().Any() && FindPerk(pred,
                         prey.SpecialDigestion, baseOrgan.Vore, key.OrganToVoreType(), true)))
                Debug.Log("Didn't find special perk");
            // Move prey to normal mode?
        }

        bool HandleOrganRegurgitation(BaseCharacter pred, string organName, BaseOrgan baseOrgan)
        {
            if (baseOrgan.Vore.Stretch < 1f)
                return false;
            Prey toReg = FindLeastDigested(baseOrgan.Vore.PreysIds);
            if (toReg == null || !baseOrgan.Vore.PreysIds.Remove(toReg.Identity.ID))
                return false;
            EventLog.AddEvent(
                $"{pred.Identity.FirstName}'s {organName} was to full they had no choice but to release their prey");
            // Notify
            return true;
        }

        public void AltDigestionPerk(BaseCharacter pred, string digestionMode, VoreOrgan baseOrgan, VoreType type)
        {
            if (perkOnes.TryGetValue(digestionMode, out var perk))
                perk.OnDigestionTick(pred, baseOrgan, type);
            else if (!(Level.OwnedPerks.OfType<VorePerkNewDigestionMode>().Any() &&
                       FindPerk(pred, digestionMode, baseOrgan, type)))
                GetDigestionMode(type).SetDigestionMode(0);
        }


        bool FindPerk(BaseCharacter pred, string digestionMode, VoreOrgan baseOrgan, VoreType type,
            bool dontTick = false)
        {
            var perks = Level.OwnedPerks.OfType<VorePerkNewDigestionMode>();
            IEnumerable<VorePerkNewDigestionMode> vorePerkNewDigestionModes =
                perks as VorePerkNewDigestionMode[] ?? perks.ToArray();
            if (vorePerkNewDigestionModes.All(p => p.DigestionMode != digestionMode))
                return false;
            perkOnes.Add(digestionMode, vorePerkNewDigestionModes.First(p => p.DigestionMode == digestionMode));
            if (dontTick)
                return true;
            perkOnes[digestionMode].OnDigestionTick(pred, baseOrgan, type);
            return true;
        }

        void PleasureDigestion(BaseCharacter pred, List<int> preyIds)
        {
            if (!CanPleasurePreys)
                return;
            foreach (int preysId in preyIds)
                VorePreyOrgasm(pred, preysId);
        }

        void VorePreyOrgasm(BaseCharacter pred, int preyId)
        {
            if (!VoredCharacters.PreyDict.TryGetValue(preyId, out Prey prey))
                return;
            int orgasms = prey.SexStats.GainArousal(pleasureDigestion.Value);
            if (orgasms <= 0)
                return;
            pred.DrainEssenceOfType(prey, DrainEssenceType, orgasmDrain.Value * orgasms);
        }

        Prey FindLeastDigested(IEnumerable<int> preyList)
        {
            Prey leastDigested = null;
            float least = 0;
            foreach (Prey p in VoredCharacters.GetPreys(preyList))
            {
                float percentDigested = p.Body.Weight / p.StartWeight;
                if (least < percentDigested)
                {
                    leastDigested = p;
                    least = percentDigested;
                }
            }

            return leastDigested;
        }

        public IEnumerator Load(SerializableScriptableObjectSaves toLoadVorePerkSave)
        {
            digestionStrength = new BaseConstIntStat(1);
            pleasureDigestion = new BaseConstIntStat(0);
            orgasmDrain = new BaseConstIntStat(0);
            yield return level.Load(toLoadVorePerkSave);
        }
    }
}