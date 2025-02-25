using System.Collections.Generic;
using UnityEngine;

public class CaldronMerger : MonoBehaviour
{
    [Tooltip("Time in seconds to wait before merging the ingredients")]
    [SerializeField] private int nbHalfTurnToMerge = 6;
    [SerializeField] private SpatulaDetection spatulaDetection;
    
    private readonly IngredientList _ingredients = new();
    private RecipesManager _recipesManager;

    private void Awake()
    {
        _recipesManager = Util.FindObjectOfTypeOrLogError<RecipesManager>();
        spatulaDetection.InitNbHalfTurnsToMerge(nbHalfTurnToMerge);
    }
    
    public void Merge()
    {
        var ingredientList = _ingredients.AddIngredient(IngredientType.Caldron);
        var ingredientResult = _recipesManager.GetRecipeResult(ingredientList);
        if (ingredientResult is not null)
        {
            MergeIngredients(ingredientResult);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        AbstractIngredient abstractIngredient = other.GetComponent<AbstractIngredient>();
        if (abstractIngredient != null)
        {
            spatulaDetection.ResetNbHalfTurns();
            _ingredients.AddIngredient(abstractIngredient.GetIngredientType());
        }
    }
    
    private void OnTriggerExit(Collider other)
    {
        AbstractIngredient abstractIngredient = other.GetComponent<AbstractIngredient>();
        if (abstractIngredient != null)
        {
            spatulaDetection.ResetNbHalfTurns();
            _ingredients.RemoveIngredient(abstractIngredient.GetIngredientType());
        }
    }

    private void MergeIngredients(GameObject ingredientResult)
    {
        var positionToInstantiate = new Vector3(transform.position.x, transform.position.y + 1.3f, transform.position.z + 0.55f);
        Instantiate(ingredientResult, positionToInstantiate, Quaternion.identity);
        _ingredients.Clear();
    }
}
