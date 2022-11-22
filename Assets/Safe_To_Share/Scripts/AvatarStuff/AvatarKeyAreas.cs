using System;
using System.Linq;
using UnityEngine;

namespace AvatarStuff
{
    public class AvatarKeyAreas : MonoBehaviour
    {
        public enum Area
        {
            Mouth,
            Shaft,
            Vagina,
            Anus
        }

        [field: SerializeField] public Transform Mouth { get; private set; }
        [field: SerializeField] public Transform ShaftRoot { get; private set; }
        [field: SerializeField] public Transform Vagina { get; private set; }
        [field: SerializeField] public Transform Anus { get; private set; }
#if UNITY_EDITOR
        public void OnValidate()
        {
            print($"Called OnVali on {gameObject.name}");
            Transform[] ts = transform.GetComponentsInChildren<Transform>(true);
            if (Mouth == null)
            {
                var found = ts.FirstOrDefault(c => c.gameObject.name == "LipLowerMiddle"); //transform.Find("LipLowerMiddle");
                if (found != null)
                    Mouth = found;
                else
                    print($"Mouth is null for {gameObject.name}");
            }

            if (ShaftRoot == null)
            {
                var found = ts.FirstOrDefault(c => c.gameObject.name == "Gen1");
                if (found != null)
                    ShaftRoot = found;
                else
                    print($"ShaftRoot is null for {gameObject.name}");
            }

            if (Vagina == null)
            {
                var found = ts.FirstOrDefault(c => c.gameObject.name == "Gen1");
                if (found != null)
                    Vagina = found;
                else
                    print($"Vagina is null for {gameObject.name}");
            }

            if (Anus == null)
            {
                var found = ts.FirstOrDefault(c => c.gameObject.name == "Anus");
                if (found != null)
                    Anus = found;
                else
                    print($"Anus is null for {gameObject.name}");
            }
        }
#endif


        public Transform GetArea(Area area) =>
            area switch
            {
                Area.Mouth => Mouth,
                Area.Shaft => ShaftRoot,
                Area.Vagina => Vagina,
                Area.Anus => Anus,
                _ => throw new ArgumentOutOfRangeException(nameof(area), area, null)
            };
    }
}