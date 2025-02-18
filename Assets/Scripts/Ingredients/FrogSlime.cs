using UnityEditor;
using UnityEngine;

public class FrogSlime : AbstractIngredient
{
    public override IngredientType GetIngredientType()
    {
        return IngredientType.FrogSlime;
    }
}
