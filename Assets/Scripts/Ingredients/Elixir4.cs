public class Elixir4 : AbstractIngredient
{
    public override IngredientType GetIngredientType()
    {
        return IngredientType.Elixir4;
    }
    
    public override bool IsElixir()
    {
        return true;
    }
}
