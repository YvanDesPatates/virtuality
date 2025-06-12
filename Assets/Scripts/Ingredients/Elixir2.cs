public class Elixir2 : AbstractIngredient
{
    public override IngredientType GetIngredientType()
    {
        return IngredientType.Elixir2;
    }
    
    public override bool IsElixir()
    {
        return true;
    }
}
