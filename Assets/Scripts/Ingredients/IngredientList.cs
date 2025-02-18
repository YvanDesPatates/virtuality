using System.Collections.Generic;
using System.Linq;

/// <summary>
/// This class is helpfull to manage a list of ingredients. Especially to easily compare two lists of ingredients.
/// </summary>
public class IngredientList
{
    private readonly List<IngredientType> _ingredients = new();
    
    public IngredientList()
    {
    }

    public IngredientList(List<AbstractIngredient> ingredients)
    {
        foreach (AbstractIngredient ingredient in ingredients)
        {
            _ingredients.Add(ingredient.GetIngredientType());
        }
    }
        
    public IngredientList AddIngredient(IngredientType ingredient)
    {
        _ingredients.Add(ingredient);
        return this;
    }
    
    public void RemoveIngredient(IngredientType ingredient)
    {
        _ingredients.Remove(ingredient);
    }
    
    public void Clear()
    {
        _ingredients.Clear();
    }
        
    // rewrite equal and hashcode to say : if the list of ingredients is the same, then the objects are the same, whatever the order of the ingredients
    public override bool Equals(object obj)
    {
        if (obj == null || GetType() != obj.GetType())
        {
            return false;
        }
        IngredientList other = (IngredientList)obj;
        if (_ingredients.Count != other._ingredients.Count)
        {
            return false;
        }
        if (!ContainsAllItems(_ingredients, other._ingredients) || !ContainsAllItems(other._ingredients, _ingredients))
        {
            return false;
        }
        return true;
    }
        
    public override int GetHashCode()
    {
        int hash = 0;
        foreach (IngredientType ingredient in _ingredients)
        {
            hash += ingredient.GetHashCode();
        }
        return hash;
    }
        
    /// <summary>
    /// return true if the list a contains all the elements of the list b
    /// </summary>
    private bool ContainsAllItems<T>(IEnumerable<T> a, IEnumerable<T> b)
    {
        return !b.Except(a).Any();
    }
}