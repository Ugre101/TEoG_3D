using CustomClasses;
using UnityEngine;

namespace SceneStuff
{
    [CreateAssetMenu(fileName = "TwoWayExit", menuName = "Scene Data/Exits", order = 0)]
    public sealed class SceneTeleportExit : SerializableScriptableObject
    {
        public Vector3 ExitPos { get; private set; }
        public void SetExit(Vector3 pos) => ExitPos = pos;
    }
}