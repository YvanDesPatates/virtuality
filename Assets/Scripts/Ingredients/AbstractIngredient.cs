using UnityEngine;

public abstract class AbstractIngredient: MonoBehaviour
{
    public abstract IngredientType GetIngredientType();

    public virtual bool IsElixir()
    {
        return false;
    }
}
