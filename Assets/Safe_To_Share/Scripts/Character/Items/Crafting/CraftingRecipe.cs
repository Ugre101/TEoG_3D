using CustomClasses;
using Items;
using UnityEngine;

namespace Safe_To_Share.Scripts.Character.Items.Crafting {
    [CreateAssetMenu(fileName = "Crafting recipe", menuName = "Items/Crafting/New Crafting Recipe", order = 0)]
    public sealed class CraftingRecipe : ScriptableObject {
        [field: SerializeField] public DropSerializableObject<Item> FirstItem { get; private set; }
        [field: SerializeField] public DropSerializableObject<Item> SecondItem { get; private set; }
        [field: SerializeField] public DropSerializableObject<Item> Result { get; private set; }
    }
}