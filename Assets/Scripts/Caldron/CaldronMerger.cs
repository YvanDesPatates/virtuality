using System;
using UnityEngine;

public class CaldronMerger : MonoBehaviour
{
    [Tooltip("number of half turn the spatula has to do before merging the ingredients")]
    [SerializeField] private int nbHalfTurnToMerge = 6;
    [Space]
    [SerializeField] private SpatulaDetection spatulaDetection;
    [SerializeField] private Rigidbody caldronRigidbody;
    
    private readonly IngredientList _ingredients = new();
    private RecipesManager _recipesManager;
    private bool _resetRotation = false;
    private Action _callbackForRotationEnd;

    private void Awake()
    {
        _recipesManager = Util.FindObjectOfTypeOrLogError<RecipesManager>();
        spatulaDetection.InitNbHalfTurnsToMerge(nbHalfTurnToMerge);
    }

    private void FixedUpdate()
    {
        if (_resetRotation)
        {
            transform.localRotation = Quaternion.Slerp(transform.localRotation, Quaternion.Euler(0, 0, 0), 0.25f);
            _resetRotation = transform.localRotation != Quaternion.Euler(0, 0, 0);
            // if the rotation is fully reset, we can call the callback to notify the animation trigger that the rotation is done
            if (!_resetRotation)
            {
                _callbackForRotationEnd?.Invoke();
            }
        }
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

    public void OnRotationAnimationStart()
    {
        caldronRigidbody.isKinematic = true;
    }

    public void OnRotationAnimationEnd(Action callback)
    {
        
        caldronRigidbody.isKinematic = false; 
        _resetRotation = true;
        _callbackForRotationEnd = callback;
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
