public class Elixir3 : AbstractIngredient
{
    public override IngredientType GetIngredientType()
    {
        return IngredientType.Elixir3;
    }
    
    public override bool IsElixir()
    {
        return true;
    }
}
