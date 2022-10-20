using System;
using System.Linq;
using Character.Organs;
using Character.Organs.OrgansContainers;
using Character.StatsStuff.Mods;
using Character.VoreStuff.VoreDigestionModes;
using Character.VoreStuff.VorePerks;
using UnityEngine;

namespace Character.VoreStuff
{
    public static class VoreSystemExtension
    {
        public static float OrganVoreCapacity(BaseCharacter pred, BaseOrgan organ)
        {
            float flatBonus = pred.Vore.capacityBoost.GetValueOfType(ModType.Flat);
            float percentBonus = 1f + organ.Vore.VoreExpMod +
                                 pred.Vore.capacityBoost.GetValueOfType(ModType.Percent) / 100f;
            return organ.ScaledWithHeight(pred.Body.Height.Value) * percentBonus + flatBonus;
        }

        public static float OralVoreCapacity(BaseCharacter pred)
        {
            float flatBonus = pred.Vore.capacityBoost.GetValueOfType(ModType.Flat);
            float percentBonus = 1f + pred.Vore.Stomach.VoreExpMod +
                                 pred.Vore.capacityBoost.GetValueOfType(ModType.Percent) / 100f;
            return pred.Body.Height.Value / 2f * percentBonus + flatBonus;
        }

        public static float AnalVoreCapacity(BaseCharacter pred,BaseOrgan organ)
        {
            float flatBonus = pred.Vore.capacityBoost.GetValueOfType(ModType.Flat);
            float percentBonus = 1f + organ.Vore.VoreExpMod +
                                 pred.Vore.capacityBoost.GetValueOfType(ModType.Percent) / 100f;
            return pred.Body.Height.Value / 3f * percentBonus + flatBonus;
        }
        public static bool CanOralVore(BaseCharacter pred, BaseCharacter prey)
        {
            float preyWeight = VoredCharacters.CurrentPreyTotalWeight(pred.Vore.Stomach.PreysIds);
            float capacity = OralVoreCapacity(pred) - preyWeight;
            return capacity >= prey.Body.Weight;
        }


        public static bool CanOrganVore(BaseCharacter pred, BaseOrgan organ, BaseCharacter prey)
        {
            float preyWeight = VoredCharacters.CurrentPreyTotalWeight(organ.Vore.PreysIds);
            float capacity = OrganVoreCapacity(pred, organ) - preyWeight;
            return capacity >= prey.Body.Weight;
        }

        public static bool CanAnalVore(BaseCharacter pred, BaseOrgan organ, BaseCharacter prey)
        {
            float preyWeight = VoredCharacters.CurrentPreyTotalWeight(organ.Vore.PreysIds);
            float capacity = AnalVoreCapacity(pred, organ) - preyWeight;
            return capacity >= prey.Body.Weight;
        }
        
        public static bool OralVore(this BaseCharacter pred, BaseCharacter prey)
        {
            if (!CanOralVore(pred, prey))
                return false;
            pred.Vore.Stomach.Vore(prey);
            pred.Vore.Stomach.SetStretch(OralVoreCapacity(pred));
            return true;
        }
        public static bool OrganVore(this BaseCharacter pred, BaseCharacter prey, SexualOrganType organType)
        {
            if (!pred.SexualOrgans.Containers.TryGetValue(organType, out OrgansContainer container) ||
                !container.HaveAny())
                return false;
            if (organType == SexualOrganType.Anal)
            {
                AnalVore(pred, prey);
            }
            foreach (BaseOrgan baseOrgan in container.List.Where(baseOrgan => CanOrganVore(pred, baseOrgan, prey)))
            {
                baseOrgan.Vore.Vore(prey);
                baseOrgan.Vore.SetStretch(OrganVoreCapacity(pred, baseOrgan));
                return true;
            }

            return false;
        }

        public static bool AnalVore(this BaseCharacter pred, BaseCharacter prey)
        {
            if (!pred.SexualOrgans.Containers.TryGetValue(SexualOrganType.Anal, out OrgansContainer container) ||
                !container.HaveAny())
                return false;
            foreach (BaseOrgan baseOrgan in container.List.Where(baseOrgan => CanAnalVore(pred, baseOrgan, prey)))
            {
                baseOrgan.Vore.Vore(prey);
                baseOrgan.Vore.SetStretch(OrganVoreCapacity(pred, baseOrgan));
                return true;
            }

            return false;
        }

        public static bool SpecialOrganVore(this BaseCharacter pred, BaseCharacter prey, SexualOrganType organType,
            SpecialVoreOptions specialVoreOptions, bool onePreyOnly)
        {
            if (!pred.SexualOrgans.Containers.TryGetValue(organType, out OrgansContainer container) ||
                !container.HaveAny())
                return false;
            foreach (BaseOrgan baseOrgan in container.List)
            {
                if (onePreyOnly && baseOrgan.Vore.SpecialPreysIds.Count > 0)
                    continue;
                Prey myPrey = new(prey);
                // if (HandleInsta(pred,myPrey, organType, specialVoreOptions, baseOrgan))
                //     return true;
                baseOrgan.Vore.SpecialPreysIds.Add(prey.Identity.ID);
                VoredCharacters.AddPrey(myPrey);
                myPrey.SetSpecialDigestionMode(GetMode());

                string GetMode() => specialVoreOptions switch
                {
                    SpecialVoreOptions.Ctf => VoreOrganDigestionMode.Absorption,
                    SpecialVoreOptions.BoobsTf => VoreOrganDigestionMode.Absorption,
                    _ => string.Empty,
                };

                return true;
            }

            return false;
        }

        static bool HandleInsta(BaseCharacter pred, Prey prey, SexualOrganType organType,
            SpecialVoreOptions specialVoreOptions, BaseOrgan baseOrgan)
        {
            if (specialVoreOptions != SpecialVoreOptions.Ctf && specialVoreOptions != SpecialVoreOptions.BoobsTf ||
                !SexualOrganAbsorption.CanInstaMorph(prey, baseOrgan)) return false;
            Debug.Log("Insta morph");
            SexualOrganAbsorption.AddToOrgan(pred, baseOrgan, prey, organType);
            // Insta morph and skip the rest
            return true;

        }

        public static bool VoreOfType(this BaseCharacter pred, BaseCharacter prey, VoreType voreType)
        {
            switch (voreType)
            {
                case VoreType.Oral:
                    if (OralVore(pred, prey))
                        return true;
                    break;
                case VoreType.Balls:
                    if (OrganVore(pred, prey, SexualOrganType.Balls))
                        return true;
                    break;
                case VoreType.UnBirth:
                    if (OrganVore(pred, prey, SexualOrganType.Vagina))
                        return true;
                    break;
                case VoreType.Anal:
                    if (AnalVore(pred, prey))
                        return true;
                    break;
                case VoreType.Breast:
                    if (OrganVore(pred, prey, SexualOrganType.Boobs))
                        return true;
                    break;
                case VoreType.Cock:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(voreType), voreType, null);
            }

            return false;
        }

        public static bool CanDoOfType(BaseCharacter pred, BaseCharacter prey, VoreType voreType) =>
            voreType switch
            {
                VoreType.Oral => CanOralVore(pred, prey),
                VoreType.Balls => pred.SexualOrgans.Balls.List.Any(baseOrgan => CanOrganVore(pred, baseOrgan, prey)),
                VoreType.UnBirth =>
                    pred.SexualOrgans.Vaginas.List.Any(baseOrgan => CanOrganVore(pred, baseOrgan, prey)),
                VoreType.Anal => pred.SexualOrgans.Anals.List.Any(baseOrgan => CanAnalVore(pred,baseOrgan,prey)),
                VoreType.Breast => pred.SexualOrgans.Boobs.List.Any(baseOrgan => CanOrganVore(pred, baseOrgan, prey)),
                _ => false,
            };

        public static void RegurgitatePrey(BaseCharacter pred, VoreType from, int id)
        {
            switch (from)
            {
                case VoreType.Oral:
                    pred.Vore.Stomach.ReleasePrey(id);
                    break;
                case VoreType.Cock:
                case VoreType.Balls:
                    foreach (BaseOrgan baseOrgan in pred.SexualOrgans.Balls.List)
                        baseOrgan.Vore.ReleasePrey(id);
                    break;
                case VoreType.UnBirth:
                    foreach (BaseOrgan baseOrgan in pred.SexualOrgans.Vaginas.List)
                        baseOrgan.Vore.ReleasePrey(id);
                    break;
                case VoreType.Anal:
                    foreach (BaseOrgan baseOrgan in pred.SexualOrgans.Anals.List)
                        baseOrgan.Vore.ReleasePrey(id);
                    break;
                case VoreType.Breast:
                    foreach (BaseOrgan baseOrgan in pred.SexualOrgans.Boobs.List)
                        baseOrgan.Vore.ReleasePrey(id);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(from), from, null);
            }
        }
    }
}