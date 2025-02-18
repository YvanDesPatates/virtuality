using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class RecipesManager : MonoBehaviour
{
    private readonly Dictionary<IngredientType, GameObject> _ingredientsPrefabs = new();
    private Dictionary<IngredientType, IngredientList> _recipes;

    private void Awake()
    {
        // register each ingredient prefab
        foreach (IngredientType ingredient in Enum.GetValues(typeof(IngredientType)))
        {
            String ingredientName = Enum.GetName(typeof(IngredientType), ingredient);
            String path = $"Assets/Prefabs/Ingredients/{ingredientName}.prefab";
            _ingredientsPrefabs[ingredient] = AssetDatabase.LoadAssetAtPath<GameObject>(path);
            if (_ingredientsPrefabs[ingredient] is null)
            {
                _ingredientsPrefabs.Remove(ingredient);
                Debug.LogWarning(
                    $"The ingredient prefab {ingredientName} was not found, be sure that is a normal behaviour");
            }
        }

        _recipes = GetRecipes();
    }

    public GameObject GetRecipeResult(IngredientList ingredients)
    {
        foreach (KeyValuePair<IngredientType, IngredientList> recipe in _recipes)
        {
            if (recipe.Value.Equals(ingredients))
            {
                return _ingredientsPrefabs[recipe.Key];
            }
        }

        return null;
    }

    private Dictionary<IngredientType, IngredientList> GetRecipes()
    {
        return new Dictionary<IngredientType, IngredientList>
        {
            {
                IngredientType.Elixir1,
                new IngredientList().AddIngredient(IngredientType.Caldron).AddIngredient(IngredientType.FrogSlime)
                    .AddIngredient(IngredientType.LysFlower)
            },
            {
                IngredientType.Elixir2,
                new IngredientList().AddIngredient(IngredientType.Caldron).AddIngredient(IngredientType.Elixir1)
                    .AddIngredient(IngredientType.Elixir1).AddIngredient(IngredientType.Elixir1)
            }
        };
    }
}