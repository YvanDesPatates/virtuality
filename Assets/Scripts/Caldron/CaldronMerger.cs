using System;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class CaldronMerger : MonoBehaviour
{
    [Tooltip("number of half turn the spatula has to do before merging the ingredients")]
    [SerializeField] private int nbHalfTurnToMerge = 6;
    [Space]
    [SerializeField] private SpatulaDetection spatulaDetection;
    [SerializeField] private Transform caldronTransform;
    [SerializeField] private AudioSource waterEmptyingSound;
    [SerializeField] private CaldronShaderController caldronShaderController;
    
    private readonly IngredientList _ingredients = new();
    private RecipesManager _recipesManager;
    private GameObject _recipeResult;
    
    private void Awake()
    {
        _recipesManager = Util.FindObjectOfTypeOrLogError<RecipesManager>();
        spatulaDetection.InitNbHalfTurnsToMerge(nbHalfTurnToMerge);
    }

    /// <summary>
    /// when called, all the ingredients are merged into the corresponding recipe result, if there is one.
    /// </summary>
    public void FinishRecipe()
    {
        var ingredientList = _ingredients.AddIngredient(IngredientType.Caldron);
        var ingredientResult = _recipesManager.GetRecipeResult(ingredientList);
        if (ingredientResult is not null)
        {
            MergeIngredients(ingredientResult);
        }
    }

    public void OnRotationMaxAngleReached()
    {
        Empty();
    }

    /// <summary>
    /// each time an ingredient enters the caldron, it is added to the list of ingredients.
    /// if there is a recipe result waiting to be transferred in a bottle, it is set to null.
    /// </summary>
    private void OnTriggerEnter(Collider other)
    {
        AbstractIngredient abstractIngredient = other.GetComponent<AbstractIngredient>();
        if (abstractIngredient != null)
        {   
            spatulaDetection.ResetNbHalfTurns();
            if(_ingredients.Count() == 0)
            {
                caldronShaderController.OnIngredientAdded();
            }
            _ingredients.AddIngredient(abstractIngredient.GetIngredientType());
            _recipeResult = null;
            return;
        }

        if (other.CompareTag("Empty_Bottle"))
        {
            ReplaceEmptyBottleWithLastRecipe(other.gameObject);
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
        _recipeResult = ingredientResult;
        _ingredients.Clear();
    }

    private void Empty()
    {
        _ingredients.Clear();
        _recipeResult = null;
        waterEmptyingSound.Play();
        caldronShaderController.OnCaldronEmptied();
    }
    
    /// <summary>
    /// fill a bottle with the last recipe result. Set the recipe result to null because a recipe fill only one bottle.
    /// </summary>
    private void ReplaceEmptyBottleWithLastRecipe(GameObject emptyBottle)
    {
        if (_recipeResult is null) return;
        var grabInteractable = emptyBottle.GetComponent<PersonalizedGrabInteractable>();
        if (grabInteractable is null) return;
        
        var position = emptyBottle.transform.position;
        var rotation = emptyBottle.transform.rotation;
        var interactor = grabInteractable.DetachInteractor();
        Destroy(emptyBottle);
        var recipeResult = Instantiate(_recipeResult, position, rotation);
        recipeResult.GetComponent<PersonalizedGrabInteractable>().AttachInteractor(interactor);

        _recipeResult = null;
    }
}
