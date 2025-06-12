public class Elixir1 : AbstractIngredient
{
    public override IngredientType GetIngredientType()
    {
        return IngredientType.Elixir1;
    }
    
    public override bool IsElixir()
    {
        return true;
    }
}
