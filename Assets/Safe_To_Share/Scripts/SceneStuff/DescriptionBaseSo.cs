using CustomClasses;
using UnityEngine;

namespace SaveStuff {
    /// <summary>
    ///     Base class for ScriptableObjects that need a public description field.
    /// </summary>
    public class DescriptionBaseSo : SerializableScriptableObject {
        [TextArea] public string description;
    }
}