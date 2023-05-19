using System.Collections.Generic;
using UnityEngine;

namespace Safe_To_Share.Scripts.Character.Items.Crafting
{
    [CreateAssetMenu(fileName = "Crafting recipe dict", menuName = "Items/Crafting/New Crafting Dict", order = 0)]
    public sealed class CraftingRecipesDict : ScriptableObject
    {
        [SerializeField] List<CraftingRecipe> craftingRecipes = new();
        Dictionary<string, Dictionary<string, CraftingRecipe>> dict;

        Dictionary<string, Dictionary<string, CraftingRecipe>> Dict
        {
            get
            {
                if (dict != null) return dict;
                dict = new Dictionary<string, Dictionary<string, CraftingRecipe>>();
                foreach (var recipe in craftingRecipes)
                {
                    if (dict.TryAdd(recipe.FirstItem.guid, new Dictionary<string, CraftingRecipe>()))
                    {
                    }
                    if (dict.TryGetValue(recipe.FirstItem.guid, out var subDict))
                        subDict.TryAdd(recipe.SecondItem.guid, recipe);
                }

                return dict;
            }
        }

        public bool TryGetResult(string item1, string item2, out CraftingRecipe recipe)
        {
            if (Dict.TryGetValue(item1, out var subDict))
            {
                if (subDict.TryGetValue(item2, out var result))
                {
                    recipe = result;
                    return true;
                }
            }
            if (Dict.TryGetValue(item2, out var subDict2))
            {
                if (subDict2.TryGetValue(item1, out var result2))
                {
                    recipe = result2;
                    return true;
                }
            }
            recipe = null;
            return false;
        }
    }
}