using UnityEngine;

public class RandomElixirPicker
{
    private static int minElixirIndex = 4; // Elixir1
    private static int maxElixirIndex = 7; // Elixir4
    
    public static IngredientType GetRandomElixirIndex()
    {
        return (IngredientType)Random.Range(minElixirIndex, maxElixirIndex + 1);
    }
}
