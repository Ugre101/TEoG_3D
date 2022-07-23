using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Character.VoreStuff
{
    [Serializable]
    public class VoreOrgan
    {
        static int timesPleadedThisSession;
        [SerializeField] List<int> preysIds = new();
        [SerializeField] List<int> specialPreysIds = new();
        [SerializeField] int voreExp;
        public List<int> PreysIds => preysIds;
        public List<int> SpecialPreysIds => specialPreysIds;
        public float Stretch { get; private set; }

        public int VoreExp
        {
            get => voreExp;
            private set => voreExp = value;
        }

        public float VoreExpMod => Mathf.Max(0f, Mathf.Log(VoreExp) * 0.1f);

        public void SetStretch(float capacity)
        {
            float stomachPreys = VoredCharacters.CurrentPreyTotalWeight(PreysIds);
            Stretch = stomachPreys / capacity;
        }


        public Prey Vore(BaseCharacter prey)
        {
            Prey item = new(prey);
            VoredCharacters.AddPrey(item);
            PreysIds.Add(item.Identity.ID);
            return item;
        }

        public float DigestTick(float toDigest, float stretch, Action<Prey> digested, bool predIsPlayer)
        {
            float v = 0;
            for (int index = PreysIds.Count; index-- > 0;)
                v = DigestPreyAndReturnAmount(toDigest, digested, v, index, predIsPlayer);
            if (stretch > 0.5f)
                VoreExp += Mathf.RoundToInt(3f * stretch);
            return v;
        }

        float DigestPreyAndReturnAmount(float toDigest, Action<Prey> digested, float v, int index, bool predIsPlayer)
        {
            int preysId = PreysIds[index];
            if (!VoredCharacters.PreyDict.TryGetValue(preysId, out Prey prey))
                return v;
            float preyDigest = prey.Digest(toDigest, predIsPlayer);
            v += preyDigest;
            if (preyDigest < toDigest)
            {
                digested?.Invoke(prey);
                RemovePrey(preysId);
            }
            else if (predIsPlayer && !prey.havePleaded && prey.DigestionProgress is > 20f and < 40f)
            {
                // 50% to trigger once then lower to not spam player
                bool shouldTrigger = Random.value / timesPleadedThisSession > 0.8f;
                if (!shouldTrigger) return v;
                timesPleadedThisSession++;
                prey.havePleaded = true;
                PleadEvent?.Invoke(prey, this);
            }

            return v;
        }

        public static event Action<Prey, VoreOrgan> PleadEvent;
        public static event Action<Prey> ReleasedPrey;

        public void ReleasePrey(int id)
        {
            if (!VoredCharacters.PreyDict.TryGetValue(id, out var prey)) return;
            if (!preysIds.Remove(id) || specialPreysIds.Remove(id)) return;
            ReleasedPrey?.Invoke(prey);
        }

        public void RemovePrey(int id)
        {
            preysIds.Remove(id);
            specialPreysIds.Remove(id);
        }

        public void NoDigestTick()
        {
            if (Stretch > 0.5f)
                VoreExp += Mathf.RoundToInt(2f * Stretch);
        }

        public void TickHour(int ticks = 1)
        {
            foreach (int preysId in PreysIds)
            {
                if (!VoredCharacters.PreyDict.TryGetValue(preysId, out var prey)) continue;
                prey.TickHour(ticks);
            }
        }
    }
}