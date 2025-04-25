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
    
    private readonly IngredientList _ingredients = new();
    private RecipesManager _recipesManager;
    private GameObject _recipeResult;
    
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

    public void OnRotationMaxAngleReached()
    {
        Empty();
    }

    private void OnTriggerEnter(Collider other)
    {
        AbstractIngredient abstractIngredient = other.GetComponent<AbstractIngredient>();
        if (abstractIngredient != null)
        {   
            spatulaDetection.ResetNbHalfTurns();
            _ingredients.AddIngredient(abstractIngredient.GetIngredientType());
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
        waterEmptyingSound.Play();
    }
    
    private void ReplaceEmptyBottleWithLastRecipe(GameObject emptyBottle)
    {
        var grabInteractable = emptyBottle.GetComponent<PersonalizedGrabInteractable>();
        if (grabInteractable is null)
        {
            return;   
        }
        
        var position = emptyBottle.transform.position;
        var rotation = emptyBottle.transform.rotation;
        var interactor = grabInteractable.DetachInteractor();
        Destroy(emptyBottle);
        var recipeResult = Instantiate(_recipeResult, position, rotation);
        recipeResult.GetComponent<PersonalizedGrabInteractable>().AttachInteractor(interactor);
    }
}
