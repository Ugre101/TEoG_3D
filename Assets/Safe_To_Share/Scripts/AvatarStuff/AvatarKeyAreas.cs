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
            if (Application.isPlaying) return;
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
               // string[] match = { "Gen1", "Shaft1" };
                // Transform found = ts.FirstOrDefault(c => match.Contains(c.gameObject.name));
                var found = ts.FirstOrDefault(c => c.gameObject.name is "Gen1" or "shaft1");
                if (found != null)
                    ShaftRoot = found;
                else
                    print($"ShaftRoot is null for {gameObject.name}");
            }

            if (Vagina == null)
            {
                var found = ts.FirstOrDefault(c => c.gameObject.name is "Gen1" or "legsCrease");
                if (found != null)
                    Vagina = found;
                else
                    print($"Vagina is null for {gameObject.name}");
            }

            if (Anus == null)
            {
                var found = ts.FirstOrDefault(c => c.gameObject.name is "Anus" or "rectum1");
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