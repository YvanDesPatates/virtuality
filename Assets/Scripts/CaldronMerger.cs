using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class CaldronMerger : MonoBehaviour
{
    [Tooltip("Time in seconds to wait before merging the ingredients")]
    [SerializeField] private float mergeTimeInSeconds = 3;
    
    private readonly List<AbstractIngredient> _ingredients = new();
    private RecipesManager _recipesManager;
    private Coroutine _mergeIngredientsCoroutine;

    private void Awake()
    {
        _recipesManager = Util.FindObjectOfTypeOrLogError<RecipesManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        AbstractIngredient abstractIngredient = other.GetComponent<AbstractIngredient>();
        if (abstractIngredient != null)
        {
            if (_mergeIngredientsCoroutine is not null)
            {
                StopCoroutine(_mergeIngredientsCoroutine);
            }
            _ingredients.Add(abstractIngredient);
            _mergeIngredientsCoroutine = StartCoroutine(MergeIngredientsCoroutine());
        }
    }
    
    private void OnTriggerExit(Collider other)
    {
        AbstractIngredient abstractIngredient = other.GetComponent<AbstractIngredient>();
        if (abstractIngredient != null)
        {
            _ingredients.Remove(abstractIngredient);
            if (_mergeIngredientsCoroutine is not null)
            {
                StopCoroutine(_mergeIngredientsCoroutine);
            }

            if (_ingredients.Count > 0)
            {
                _mergeIngredientsCoroutine = StartCoroutine(MergeIngredientsCoroutine());
            }
        }
    }
    
    private IEnumerator MergeIngredientsCoroutine()
    {
        yield return new WaitForSeconds(3);
        
        var ingredientList = new IngredientList(_ingredients).AddIngredient(IngredientType.Caldron);
        var ingredientResult = _recipesManager.GetRecipeResult(ingredientList);
        if (ingredientResult is not null)
        {
            MergeIngredients(ingredientResult);
        }
    }

    private void MergeIngredients(GameObject ingredientResult)
    {
        var positionToInstantiate = new Vector3(transform.position.x, transform.position.y + 1, transform.position.z);
        foreach (AbstractIngredient ingredient in _ingredients)
        {
            Destroy(ingredient.gameObject);
        }
        _ingredients.Clear();
        Instantiate(ingredientResult, positionToInstantiate, Quaternion.identity);
    }
}
